from .structures import CaseInsensitiveDict

from http.cookiejar import CookieJar, Cookie
from typing import MutableMapping, Union, Any
from urllib.parse import urlparse, urlunparse
from http.client import HTTPMessage
import copy

try:
    import threading
except ImportError:
    import dummy_threading as threading


class MockRequest:
    """
    Mimic a urllib2.Request to get the correct cookie string for the request.
    """

    def __init__(self, request_url: str, request_headers: CaseInsensitiveDict):
        self.request_url = request_url
        self.request_headers = request_headers
        self._new_headers = {}
        self.type = urlparse(self.request_url).scheme

    def get_type(self):
        return self.type

    def get_host(self):
        return urlparse(self.request_url).netloc

    def get_origin_req_host(self):
        return self.get_host()

    def get_full_url(self):
        # Only return the response's URL if the user hadn't set the Host
        # header
        if not self.request_headers.get("Host"):
            return self.request_url
        # If they did set it, retrieve it and reconstruct the expected domain
        host = self.request_headers["Host"]
        parsed = urlparse(self.request_url)
        # Reconstruct the URL as we expect it
        return urlunparse(
            [
                parsed.scheme,
                host,
                parsed.path,
                parsed.params,
                parsed.query,
                parsed.fragment,
            ]
        )

    def is_unverifiable(self):
        return True

    def has_header(self, name):
        return name in self.request_headers or name in self._new_headers

    def get_header(self, name, default=None):
        return self.request_headers.get(name, self._new_headers.get(name, default))

    def add_unredirected_header(self, name, value):
        self._new_headers[name] = value

    def get_new_headers(self):
        return self._new_headers

    @property
    def unverifiable(self):
        return self.is_unverifiable()

    @property
    def origin_req_host(self):
        return self.get_origin_req_host()

    @property
    def host(self):
        return self.get_host()


class MockResponse:
    """
    Wraps a httplib.HTTPMessage to mimic a urllib.addinfourl.
    The objective is to retrieve the response cookies correctly.
    """

    def __init__(self, headers):
        self._headers = headers

    def info(self):
        return self._headers

    def getheaders(self, name):
        self._headers.getheaders(name)


class CookieConflictError(RuntimeError):
    """There are two cookies that meet the criteria specified in the cookie jar.
    Use .get and .set and include domain and path args in order to be more specific.
    """


class RequestsCookieJar(CookieJar, MutableMapping):

    def get(self, name, default=None, domain=None, path=None):

        try:
            return self._find_no_duplicates(name, domain, path)
        except KeyError:
            return default

    def set(self, name, value, **kwargs):
        if value is None:
            remove_cookie_by_name(
                self, name, domain=kwargs.get("domain"), path=kwargs.get("path")
            )
            return

        c = create_cookie(name, value, **kwargs)
        self.set_cookie(c)
        return c

    def iterkeys(self):
        for cookie in iter(self):
            yield cookie.name

    def keys(self):

        return list(self.iterkeys())

    def itervalues(self):

        for cookie in iter(self):
            yield cookie.value

    def values(self):

        return list(self.itervalues())

    def iteritems(self):

        for cookie in iter(self):
            yield cookie.name, cookie.value

    def items(self):

        return list(self.iteritems())

    def list_domains(self):
        domains = []
        for cookie in iter(self):
            if cookie.domain not in domains:
                domains.append(cookie.domain)
        return domains

    def list_paths(self):
        paths = []
        for cookie in iter(self):
            if cookie.path not in paths:
                paths.append(cookie.path)
        return paths

    def multiple_domains(self):

        domains = []
        for cookie in iter(self):
            if cookie.domain is not None and cookie.domain in domains:
                return True
            domains.append(cookie.domain)
        return False

    def get_dict(self, domain=None, path=None):

        dictionary = {}
        for cookie in iter(self):
            if (domain is None or cookie.domain == domain) and (
                path is None or cookie.path == path
            ):
                dictionary[cookie.name] = cookie.value
        return dictionary

    def __contains__(self, name):
        try:
            return super().__contains__(name)
        except CookieConflictError:
            return True

    def __getitem__(self, name):

        return self._find_no_duplicates(name)

    def __setitem__(self, name, value):

        self.set(name, value)

    def __delitem__(self, name):

        remove_cookie_by_name(self, name)

    def set_cookie(self, cookie, *args, **kwargs):
        if (
            hasattr(cookie.value, "startswith")
            and cookie.value.startswith('"')
            and cookie.value.endswith('"')
        ):
            cookie.value = cookie.value.replace('\\"', "")
        return super().set_cookie(cookie, *args, **kwargs)

    def update(self, other):
        if isinstance(other, CookieJar):
            for cookie in other:
                self.set_cookie(copy.copy(cookie))
        else:
            super().update(other)

    def _find(self, name, domain=None, path=None):

        for cookie in iter(self):
            if cookie.name == name:
                if domain is None or cookie.domain == domain:
                    if path is None or cookie.path == path:
                        return cookie.value

        raise KeyError(f"name={name!r}, domain={domain!r}, path={path!r}")

    def _find_no_duplicates(self, name, domain=None, path=None):

        toReturn = None
        for cookie in iter(self):
            if cookie.name == name:
                if domain is None or cookie.domain == domain:
                    if path is None or cookie.path == path:
                        if toReturn is not None:
                            # if there are multiple cookies that meet passed in criteria
                            raise CookieConflictError(
                                f"There are multiple cookies with name, {name!r}"
                            )
                        toReturn = cookie.value

        if toReturn:
            return toReturn
        raise KeyError(f"name={name!r}, domain={domain!r}, path={path!r}")

    def __getstate__(self):
        state = self.__dict__.copy()
        state.pop("_cookies_lock")
        return state

    def __setstate__(self, state):
        self.__dict__.update(state)
        if "_cookies_lock" not in self.__dict__:
            self._cookies_lock = threading.RLock()

    def copy(self):
        new_cj = RequestsCookieJar()
        new_cj.set_policy(self.get_policy())
        new_cj.update(self)
        return new_cj

    def get_policy(self):
        return self._policy


def remove_cookie_by_name(cookiejar: RequestsCookieJar, name: str, domain: str = None, path: str = None):
    clearables = []
    for cookie in cookiejar:
        if cookie.name != name:
            continue
        if domain is not None and domain != cookie.domain:
            continue
        if path is not None and path != cookie.path:
            continue
        clearables.append((cookie.domain, cookie.path, cookie.name))

    for domain, path, name in clearables:
        cookiejar.clear(domain, path, name)


def create_cookie(name: str, value: str, **kwargs: Any) -> Cookie:
    result = {
        "version": 0,
        "name": name,
        "value": value,
        "port": None,
        "domain": "",
        "path": "/",
        "secure": False,
        "expires": None,
        "discard": True,
        "comment": None,
        "comment_url": None,
        "rest": {"HttpOnly": None},
        "rfc2109": False,
    }

    badargs = set(kwargs) - set(result)
    if badargs:
        raise TypeError(
            f"create_cookie() got unexpected keyword arguments: {list(badargs)}"
        )

    result.update(kwargs)
    result["port_specified"] = bool(result["port"])
    result["domain_specified"] = bool(result["domain"])
    result["domain_initial_dot"] = result["domain"].startswith(".")
    result["path_specified"] = bool(result["path"])

    return Cookie(**result)


def cookiejar_from_dict(cookie_dict: dict) -> RequestsCookieJar:
    cookie_jar = RequestsCookieJar()
    if cookie_dict is not None:
        for name, value in cookie_dict.items():
            cookie_jar.set_cookie(create_cookie(name=name, value=value))
    return cookie_jar


def merge_cookies(cookiejar: RequestsCookieJar, cookies: Union[dict, RequestsCookieJar]) -> RequestsCookieJar:
    if type(cookies) is dict:
        cookies = cookiejar_from_dict(cookies)

    for cookie in cookies:
        cookiejar.set_cookie(cookie)

    return cookiejar

def extract_cookies_to_jar(
        request_url: str,
        request_headers: CaseInsensitiveDict,
        cookie_jar: RequestsCookieJar,
        response_headers: dict
    ) -> RequestsCookieJar:
    response_cookie_jar = cookiejar_from_dict({})

    req = MockRequest(request_url, request_headers)
    http_message = HTTPMessage()
    http_message._headers = []
    for header_name, header_values in response_headers.items():
        for header_value in header_values:
            http_message._headers.append(
                (header_name, header_value)
            )
    res = MockResponse(http_message)
    response_cookie_jar.extract_cookies(res, req)

    merge_cookies(cookie_jar, response_cookie_jar)
    return response_cookie_jar
