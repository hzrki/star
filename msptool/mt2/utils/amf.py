import base64
from secrets import token_hex
import requests
from pyamf import remoting, AMF3
from msptool.mt2 import tlsclient
from msptool.mt2.security.checksumCalculator import create_checksum


def AmfCall(server: str, method: str, params: list) -> tuple[int, any]:

    if server.lower() == 'UK':
        server = 'GB'

    if server.lower() == 'SV':
        server = 'SE'

    if server.lower() == 'USA':
        server = 'US'

    req = remoting.Request(target=method, body=params)
    event = remoting.Envelope(AMF3)
    event.headers = remoting.HeaderCollection({
        ("sessionID", False, base64.b64encode(token_hex(23).encode()).decode()),
        ("needClassName", False, False),
        ("id", False, create_checksum(params)
    )})
    event['/1'] = req
    encoded_req = remoting.encode(event).getvalue()

    headersdisco = {
        'Accept-Encoding': 'deflate, gzip',
        'User-Agent': 'Mozilla/5.0 (Android; U; en-GB) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/50.2',
        'x-flash-version': '50,2,3,4',
        'Connection': 'Keep-Alive',
        'Referer': 'app:/MSPMobile.swf',
        'Accept': 'application/json',
        'Content-Type': 'multipart/form-data'
    }
    disu = f"https://disco.mspapis.com/disco/v1/services/msp/{server}?services=mspwebservice"
    resp = requests.get(disu, verify=False, headers=headersdisco)
    resp = resp.json()
    endpoint = resp['Services'][0]['Endpoint']
    full_endpoint = f"{endpoint}/Gateway.aspx?method={method}"
    session = tlsclient.Session(
        client_identifier="chrome_120",
    )
    resp = session.post(full_endpoint, data=encoded_req)
    resp2 = resp.content if resp.status_code == 200 else None

    if resp.status_code != 200:
        return (resp.status_code, resp2)
    return (resp.status_code, remoting.decode(resp2)["/1"].body)

def getws(server):
    wsu = "https://presence-us.mspapis.com/getServer" if server.lower() == "us" else "https://presence.mspapis.com/getServer"

    headers = {
        "Referer": "app:/MSPMobile.swf",
        "Accept": (
            "text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5"),
        "x-flash-version": "50,2,3,4",
        "Accept-Encoding": "gzip,deflate",
        "User-Agent": "Mozilla/5.0 (Android; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/50.2"
                      "(KHTML, like Gecko) AdobeAIR/32.0",
        "Connection": "Keep-Alive",

    }

    resp = requests.get(wsu,headers=headers, verify=False)
    return resp.text
