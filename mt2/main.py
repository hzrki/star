from rich.console import Console
from rich.table import Table
from rich.prompt import Prompt
import time
import os
import requests
import sys
import shutil
from utils.amf import AmfCall
from utils.apis import (buy_boonie, buy_animation, buy_clothes, buy_eyes,
                        wear_rareskin, add_to_wishlist, custom_status,
                        recycle_items, wheel_spins, msp_query)
from utils.settings import WebServer, Loc1, Home, CURRENT_VERSION, VERSION_, RELEASE_

console = Console()

spt1 = """
                                                   .=+**+-.                        
                                                  =*######*-                       
                                                 :###*--*###.                      
                                           :--===*##*-  =###*===--.                
                                         :*#########+   .+#########+:              
                                        :*###======-.    .-=====+###*.             
                                      .:=###*-.                :=###*.             
                                 :-+**########*+:.          .-+####*:              
                             .-+*################*         .*####+:                
                           :+####################=          =###+                  
                         -*#####################+.  .-**-.  .*###-                 
                       :*#######################=:-+#####*+-.+###=                 
                     .=###################*+=########*++*#######*.                 
                    -*###################+:  .=+*#*+-.  .-+*#*+-.                  
                  .=####################-                                          
                 .+###################*:                                           
                .+###################*:                                            
               .*###################*:                                             
              .+###################*-                                              
              =####################=                                               
             .*#*+-=##############*.                                               
              ::. .+##############=                                                
                  :*######*=#####*.                                                
                  :######*: =####=                                                 
                  -#####+.  .+###-                                                 
                  -####=     .+**.                                                 
                  :*#*:        ::                                                  
                  .=+:                                                             
"""

def check_version():
        resp = requests.get(VERSION_)
        LATEST_ = resp.text.strip()
        if LATEST_ != CURRENT_VERSION:
            console.print(f"[bold yellow]A new version ({LATEST_}) is available! Downloading the update...[/]",
                          style="bold yellow")
            install_update(LATEST_)

def install_update(version):
    ep = RELEASE_.format(version=version)
    try:
        resp = requests.get(ep, stream=True)
        resp.raise_for_status()

        iu1 = "mt2_update.exe"

        with open(iu1, "wb") as f:
            for aaaa in resp.iter_content(chunk_size=8192):
                f.write(aaaa)

        console.print(f"[bold green]Update downloaded successfully![/]", style="bold green")
        repl_exe(iu1)

    except requests.RequestException as e:
        console.print(f"[bold red] Failed | Contact Developer if issue continues! [/]", style="bold red")
        exit()


def repl_exe(latest_e):
    current_ = sys.argv[0]
    shutil.move(latest_e, current_)
    console.print(f"[bold green]m2t updated successfully! Restarting...[/]", style="bold green")
    os.execl(sys.executable, sys.executable, *sys.argv)


def login():
    check_version()

    while True:
        console.clear()
        console.print(spt1, style="bold white")
        console.print("[#71d5fb]Star Project by ham & 6c0[/]", style="bold")

        serverMenu = Table(title="Select Server")
        serverMenu.add_column("Options", style="bold cyan")
        serverMenu.add_column("Server", style="bold cyan")

        for ws in WebServer:
            serverMenu.add_row(str(ws.value), Loc1.loc3(ws)[0])

        console.print(serverMenu)

        pickedServer = Prompt.ask("[#71d5fb]Enter server number: [/]")

        try:
            chosenServer = WebServer(int(pickedServer))
            server = Loc1.loc3(chosenServer)[1]
        except ValueError:
            console.print("choose a server that exists :)", style="bold red")
            continue
        except KeyError:
            console.print("choose a server that exists :)", style="bold red")
            continue

        username = Prompt.ask("[#71d5fb]Enter username: [/]")
        password = Prompt.ask("[#71d5fb]Enter password: [/]")

        with console.status("[#71d5fb]Loading...[/]", spinner="star") as status:
            time.sleep(15)

        code, resp = AmfCall(server,
                             "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                             [username, password, [], None, None, "MSP1-Standalone:XXXXXX"])

        logged_in = resp.get('loginStatus', {}).get('status')
        locked_status = resp.get('loginStatus', {}).get('statusDetails')

        if logged_in == "InvalidCredentials":
            console.print(f"Failed | Invalid username or password [Click any key to return to login]", style="bold red")
            Prompt.ask("Press Enter to try again...")

        elif locked_status == "LockPermanent":
            console.print(f"Failed | Your account is permanently locked [Click any key to return to login]",
                          style="bold red")
            Prompt.ask("Press Enter to try again...")

        elif logged_in == "Success":
            console.print(f"Success | Logged in {username}! Press any key to continue...", style="bold green")

            actorId = resp['loginStatus']['actor']['ActorId']
            name = resp["loginStatus"]["actor"]["Name"]
            ticket = resp['loginStatus']['ticket']
            accessToken = resp["loginStatus"]["nebulaLoginStatus"]["accessToken"]
            profileId = resp["loginStatus"]["nebulaLoginStatus"]["profileId"]
            homeMenu(server, ticket, name, actorId, accessToken, profileId)
            break


def homeMenu(server, ticket, name, actorId, accessToken, profileId):
    while True:
        console.clear()
        displayHome = Table(title="Menu Options")
        displayHome.add_column("Options", style="bold cyan")
        displayHome.add_column("Extensions", style="bold cyan")

        for option, Extensions in Home:
            displayHome.add_row(str(option), Extensions)
        console.print(displayHome)

        chosenop = Prompt.ask("[#71d5fb]Select an extension [/]")

        try:
            options = int(chosenop)
            if 1 <= options <= 11:
                if options == 1:
                    buy_boonie(server, ticket, actorId)
                if options == 2:
                    buy_animation(server, ticket, actorId)
                if options == 3:
                    buy_clothes(server, ticket, actorId)
                if options == 4:
                    buy_eyes(server, ticket, actorId)
                if options == 5:
                    wear_rareskin(server, ticket, actorId)
                if options == 6:
                    add_to_wishlist(server, ticket)
                if options == 7:
                    custom_status(server, ticket, actorId, name)
                if options == 8:
                    recycle_items(server, ticket, actorId)
                if options == 9:
                    wheel_spins(server, ticket, actorId)
                if options == 10:
                    msp_query(server, ticket, actorId)
                elif options == 11:
                    console.print("Logging out...", style="bold green")
                    break
            else:
                console.print("Please type an existing extension : )", style="bold red")
        except ValueError:
            console.print("Please type an existing extension : )", style="bold red")

        console.print("Press any key to return to home...", style="bold yellow")
        Prompt.ask("")


if __name__ == "__main__":
    login()