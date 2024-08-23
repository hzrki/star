from .cookies import cookiejar_from_dict, RequestsCookieJar
from .structures import CaseInsensitiveDict
from typing import Union
import json
import base64


class Response:
    def __init__(self):
        self.url = None
        self.status_code = None
        self.text = None
        self.headers = CaseInsensitiveDict()
        self.cookies = cookiejar_from_dict({})
        self._content = False
        self._content_consumed = False

    def __enter__(self):
        return self

    def __repr__(self):
        return f"<Response [{self.status_code}]>"

    def json(self, **kwargs):
        return json.loads(self.text, **kwargs)

    @property
    def content(self):
        if self._content is False:
            if self._content_consumed:
                raise RuntimeError("The content for this response was already consumed")
            self._content = b"".join(self.iter_content(10 * 1024)) or b""
        self._content_consumed = True
        return self._content


def build_response(res: Union[dict, list], res_cookies: RequestsCookieJar) -> Response:
    response = Response()
    response.url = res.get("target")
    response.status_code = res.get("status")
    response.headers = {
        header_key: header_value[0] if len(header_value) == 1 else header_value
        for header_key, header_value in res.get("headers", {}).items()
    }
    response.cookies = res_cookies
    body = res.get("body", "")
    if body.startswith('data:application/octet-stream;base64,'):
        body = body.split(',', 1)[1]

    response.text = body
    response._content = base64.b64decode(body.encode())

    return response
