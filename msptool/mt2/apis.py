from rich.console import Console
from rich.prompt import Prompt
from AmfCall import AmfCall
from ticketHeader import ticketHeader
console = Console()


def buy_clothes(server, ticket, actorId):
    rare_id = Prompt.ask("[#71d5fb]Enter Clothes ID: [/]")
    color = Prompt.ask("[#71d5fb]Enter Color: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.AMFSpendingService.BuyClothes",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            [{"Color": color,
              "x": 0,
              "ClothesId": rare_id,
              "ActorClothesRelId": 0,
              "y": 0,
              "IsWearing": 1,
             "ActorId": actorId}],
            0
        ],
    )
    description = resp.get('Description', '')

    if code == 500:
        console.print("FAILED | Not allowed to spawn item", style="bold red")
    elif description:
        console.print(f"FAILED | {description}", style="bold red")
    else:
        console.print("SUCCESS | Clothing item bought!", style="bold green")
