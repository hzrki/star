from rich.console import Console
from rich.prompt import Prompt
from mt2.utils.amf import AmfCall
from mt2.security.ticketHeader import ticketHeader
console = Console()

def buy_boonie(server, ticket, actorId):
    boonie_id = Prompt.ask("[#71d5fb]Enter Boonie ID: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Pets.AMFPetService.BuyClickItem",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            boonie_id
        ],
    )

def buy_animation(server, ticket, actorId):
    animation_id = Prompt.ask("[#71d5fb]Enter Animation ID: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            animation_id
        ],
    )


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
