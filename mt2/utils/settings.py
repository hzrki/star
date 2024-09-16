from enum import Enum
from typing import Tuple, Dict
from curl_cffi import requests
import shutil
import os
import sys

VERSION_ = "https://raw.githubusercontent.com/r-h-y/star/main/msptool/version.txt"
RELEASE_ = "https://github.com/r-h-y/star/releases/download/v{version}/mt2.exe"
CURRENT_VERSION = "2024.4.1.2"


def check_version(status_label):
        resp = requests.get(VERSION_)
        LATEST_ = resp.text.strip()
        if LATEST_ != CURRENT_VERSION:
            status_label.setText(f"A new version ({LATEST_}) is available! Downloading the update...")
            install_update(LATEST_, status_label)
        else:
            status_label.setText("You are on the latest version.")


def install_update(version, status_label):
    ep = RELEASE_.format(version=version)
    resp = requests.get(ep, stream=True)

    iu1 = "mt2_update.exe"
    with open(iu1, "wb") as f:
        for chunk in resp.iter_content(chunk_size=8192):
                f.write(chunk)

        status_label.setText("Update downloaded successfully!")
        repl_exe(iu1, status_label)


def repl_exe(latest_e, status_label):
    current_ = sys.argv[0]
    shutil.move(latest_e, current_)
    status_label.setText("mt2 updated successfully! Restarting...")
    os.execl(sys.executable, sys.executable, *sys.argv)

Home = [
    (1, "Buy Boonie"),
    (2, "Buy Animations"),
    (3, "Buy Clothes"),
    (4, "Buy Eyes"),
    (5, "Wear RareSkin"),
    (6, "Add to wishlist"),
    (7, "Custom Status"),
    (8, "Recycler Anything (Diamond items, clothes etc)"),
    (9, "Wheel Spins"),
    (10, "Query"),
    (11, "Logout")
]


class WebServer(Enum):
    UnitedKingdom = 1
    UnitedStates = 2
    Türkiye = 3
    Sweden = 4
    France = 5
    Deutschland = 6
    Netherlands = 7
    Finland = 8
    Norway = 9
    Denmark = 10
    Canada = 11
    Australia = 12
    Poland = 13
    NewZealand = 14
    Ireland = 15
    Spain = 16

class Loc1:
    loc2: Dict[WebServer, Tuple[str, str]] = {
        WebServer.UnitedKingdom: ("United Kingdom", "GB"),
        WebServer.UnitedStates: ("United States", "US"),
        WebServer.Türkiye: ("Türkiye", "TR"),
        WebServer.Sweden: ("Sweden", "SE"),
        WebServer.France: ("France", "FR"),
        WebServer.Deutschland: ("Deutschland", "DE"),
        WebServer.Netherlands: ("Netherlands", "NL"),
        WebServer.Finland: ("Finland", "FI"),
        WebServer.Norway: ("Norway", "NO"),
        WebServer.Denmark: ("Denmark", "DK"),
        WebServer.Canada: ("Canada", "CA"),
        WebServer.Australia: ("Australia", "AU"),
        WebServer.Poland: ("Poland", "PL"),
        WebServer.NewZealand: ("New Zealand", "NZ"),
        WebServer.Ireland: ("Ireland", "IE"),
        WebServer.Spain: ("Spain", "ES")
    }

    @staticmethod
    def loc3(web_server: WebServer) -> Tuple[str, str]:
        return Loc1.loc2.get(web_server, ("Unknown", "Unknown"))
