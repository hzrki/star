from enum import Enum
from typing import Tuple, Dict


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