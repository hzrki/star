from rich.console import Console
from rich.table import Table
from rich.prompt import Prompt
import time
from msptool.mt2.utils.amf import AmfCall
from msptool.mt2.utils.localisation import Home
from msptool.mt2.utils.webserver import WebServer, Loc1
from msptool.mt2.utils.apis import buy_clothes

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

def login():
    while True:
        console.clear()
        console.print(spt1, style="bold white")
        console.print("[#71d5fb]Star Project by lcfi & 6c0[/]", style="bold")

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
            console.print("choose an server that exists :)", style="bold red")
            continue
        except KeyError:
            console.print("choose an server that exists :)", style="bold red")
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
            if 1 <= options <= 32:
                if options == 4:
                    buy_clothes(server, ticket, actorId)
                elif options == 32:
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
