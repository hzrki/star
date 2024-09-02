from rich.console import Console
from rich.table import Table
from rich.prompt import Prompt
import time
from utils.amf import AmfCall
from utils.apis import buy_boonie, buy_animation, buy_clothes, buy_eyes, buy_nose, buy_lips, \
    wear_rareskin, add_to_wishlist, custom_status, recycle_items, wheel_spins, lisa_hack, \
    automated_pixeller, msp_query, username_checker, clothes_extractor, \
    item_tracker, room_changer, animations_extractor, icon_changer, bot_generator, item_glitcher, automated_autographer
from utils.localisation import Home
from utils.webserver import WebServer, Loc1

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
            if 1 <= options <= 31:
                if options == 1:
                    buy_boonie(server, ticket, actorId)
                if options == 2:
                    buy_animation(server, ticket, actorId)
                if options == 3:
                    buy_clothes(server, ticket, actorId)
                if options == 4:
                    buy_eyes(server, ticket, actorId)
                if options == 5:
                    buy_nose(server, ticket, actorId)
                if options == 6:
                    buy_lips(server, ticket, actorId)
                if options == 7:
                    wear_rareskin(server, ticket, actorId)
                if options == 8:
                    add_to_wishlist(server, ticket, actorId)
                if options == 9:
                    custom_status(server, ticket, actorId, name)
                if options == 10:
                    recycle_items(server, ticket, actorId)
                if options == 11:
                    wheel_spins(server, ticket, actorId)
                if options == 12:
                    lisa_hack(server, ticket, actorId)
                if options == 13:
                    automated_pixeller(server, ticket, actorId)
                if options == 14:
                    msp_query(server, ticket, actorId)
                if options == 15:
                    username_checker()
                if options == 16:
                    clothes_extractor(server, ticket)
                if options == 17:
                    item_tracker(server)
                if options == 18:
                    room_changer(server, ticket, actorId)
                if options == 19:
                    animations_extractor(server, ticket, actorId)
                if options == 20:
                    icon_changer(server, ticket, actorId)
                if options == 21:
                    bot_generator(server, ticket)
                if options == 22:
                    item_glitcher(server, ticket, actorId)
                if options == 23:
                    automated_autographer(server, ticket, actorId)
                elif options == 24:
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
