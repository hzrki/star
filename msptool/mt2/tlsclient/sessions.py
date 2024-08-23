from .cffi import request, freeMemory, destroySession
from .response import build_response, Response
from .settings import ClientIdentifiers
from .structures import CaseInsensitiveDict

from typing import Any, Dict, List, Optional, Union
from json import dumps, loads
import urllib.parse
import base64
import ctypes
import uuid

class Session:
    def __init__(
            self,
            client_identifier: ClientIdentifiers = "chrome_120",
            ja3_string: Optional[str] = None,
            force_http1: Optional[bool] = False,
    ) -> None:
        self._session_id = str(uuid.uuid4())

        self.headers = CaseInsensitiveDict({
            "Referer": "app:/cache/t1.bin/[[DYNAMIC]]/2",
            "Accept": ("text/xml, application/xml, application/xhtml+xml, "
                       "text/html;q=0.9, text/plain;q=0.8, text/css, image/png, "
                       "image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, "
                       "video/mp4;q=0.9, flv-application/octet-stream;q=0.8, "
                       "video/x-flv;q=0.7, audio/mp4, application/futuresplash, "
                       "/;q=0.5, application/x-mpegURL"),
            "x-flash-version": "32,0,0,100",
            "Content-Type": "application/x-amf",
            "Accept-Encoding": "gzip, deflate",
            "User-Agent": "Mozilla/5.0 (Windows; U; en) AppleWebKit/533.19.4 "
                          "(KHTML, like Gecko) AdobeAIR/32.0",
            "Connection": "Keep-Alive"
        })

        self.proxies = {}
        self.timeout_seconds = 30
        self.client_identifier = client_identifier
        self.ja3_string = ja3_string
        self.force_http1 = force_http1

    def __enter__(self):
        return self

    def __exit__(self, *args):
        self.close()

    def close(self) -> str:
        destroy_session_payload = {
            "sessionId": self._session_id
        }

        destroy_session_response = destroySession(dumps(destroy_session_payload).encode('utf-8'))
        destroy_session_response_bytes = ctypes.string_at(destroy_session_response)
        destroy_session_response_string = destroy_session_response_bytes.decode('utf-8')
        destroy_session_response_object = loads(destroy_session_response_string)

        freeMemory(destroy_session_response_object['id'].encode('utf-8'))

        return destroy_session_response_string

    def post(
            self,
            url: str,
            data: Optional[Union[str, dict]] = None,
            json: Optional[dict] = None,
            **kwargs: Any
    ) -> Response:
        if kwargs.get('params') is not None:
            url = f"{url}?{urllib.parse.urlencode(kwargs['params'], doseq=True)}"

        # Prepare request body
        if data is None and json is not None:
            if isinstance(json, (dict, list)):
                json = dumps(json)
            request_body = json
            content_type = "application/json"
        elif data is not None and not isinstance(data, (str, bytes)):
            request_body = urllib.parse.urlencode(data, doseq=True)
            content_type = "application/x-www-form-urlencoded"
        else:
            request_body = data
            content_type = None

        if content_type is not None and "Content-Type" not in self.headers:
            self.headers["Content-Type"] = content_type

        headers = kwargs.get('headers') or self.headers

        proxy = kwargs.get('proxy') or self.proxies
        if isinstance(proxy, dict) and "http" in proxy:
            proxy = proxy["http"]
        elif isinstance(proxy, str):
            proxy = proxy
        else:
            proxy = ""

        timeout_seconds = kwargs.get('timeout_seconds') or self.timeout_seconds

        is_byte_request = isinstance(request_body, (bytes, bytearray))
        request_payload = {
            "sessionId": self._session_id,
            "followRedirects": kwargs.get('allow_redirects', False),
            "forceHttp1": self.force_http1,
            "headers": dict(headers),
            "insecureSkipVerify": kwargs.get('insecure_skip_verify', False),
            "isByteRequest": is_byte_request,
            "isByteResponse": True,
            "proxyUrl": proxy,
            "requestUrl": url,
            "requestMethod": "POST",
            "requestBody": base64.b64encode(request_body).decode() if is_byte_request else request_body,
            "timeoutSeconds": timeout_seconds,
        }
        if self.client_identifier is not None:
            request_payload["tlsClientIdentifier"] = self.client_identifier
        if self.ja3_string is not None:
            request_payload["customTlsClient"] = {
                "ja3String": self.ja3_string
            }

        response = request(dumps(request_payload).encode('utf-8'))
        response_bytes = ctypes.string_at(response)
        response_string = response_bytes.decode('utf-8')
        response_object = loads(response_string)
        freeMemory(response_object['id'].encode('utf-8'))

        response_cookies = response_object.get("headers", {}).get("Set-Cookie", [])
        if isinstance(response_cookies, str):
            response_cookies = [response_cookies]

        return build_response(response_object, response_cookies)
