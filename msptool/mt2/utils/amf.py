import base64
from secrets import token_hex
from pyamf import remoting, AMF3, amf0
from curl_cffi import requests
from utils.checksum import create_checksum


def AmfCall(server: str, method: str, params: list) -> tuple[int, any]:

    req = remoting.Request(target=method, body=params)
    event = remoting.Envelope(AMF3)
    event.headers = remoting.HeaderCollection({
        ("sessionID", False, base64.b64encode(token_hex(23).encode()).decode()),
        ("needClassName", False, False),
        ("id", False, create_checksum(params))
    })
    event['/1'] = req
    encoded_req = remoting.encode(event).getvalue()
    full_endpoint = f"https://ws-{server}.mspapis.com/Gateway.aspx?method={method}"

    headerspost = {
        'Content-Type': 'application/x-amf',
        'User-Agent': 'Mozilla/5.0 (Android; U; en-GB) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/50.2',
        'x-flash-version': '50,2,3,4',
        'Connection': 'Keep-Alive',
        'Referer': 'app:/MSPMobile.swf',
    }

    post_resp = requests.post(full_endpoint, headers=headerspost, data=encoded_req, verify=False)
    post_resp_content = post_resp.content if post_resp.status_code == 200 else None

    if post_resp.status_code != 200:
        return (post_resp.status_code, post_resp_content)
    return (post_resp.status_code, remoting.decode(post_resp_content)["/1"].body)


def getws(server):
    wsu = "https://presence-us.mspapis.com/getServer" if server.lower() == "us" else "https://presence.mspapis.com/getServer"

    headers = {
        "Referer": "app:/MSPMobile.swf",
        "Accept": (
            "text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5"),
        "x-flash-version": "50,2,3,4",
        "Accept-Encoding": "gzip,deflate",
        "User-Agent": "Mozilla/5.0 (Android; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/50.2",
        "Connection": "Keep-Alive",
    }

    resp = requests.get(wsu, headers=headers, verify=False)
    return resp.text