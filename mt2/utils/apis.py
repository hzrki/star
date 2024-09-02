from rich.console import Console
from rich.prompt import Prompt
from utils.amf import AmfCall
from security.ticketHeader import ticketHeader
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
    if code == 500:
        console.print("FAILED | BoonieId not found", style="bold red")
    else:
        console.print("SUCCESS | Boonie bought!", style="bold green")

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
    description = resp.get('Description', '')

    if code != 500:
        console.print("FAILED | AnimationId was not found", style="bold red")
    elif description:
        console.print(f"FAILED | {description}", style="bold red")
    else:
        console.print("SUCCESS | Animation bought!", style="bold green")


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

    if code != 500:
        console.print("FAILED | Not allowed to spawn item", style="bold red")
    elif description:
        console.print(f"FAILED | {description}", style="bold red")
    else:
        console.print("SUCCESS | Clothing item bought!", style="bold green")

def buy_eyes(server, ticket, actorId):
    eye_id = Prompt.ask("[#71d5fb]Enter Eye ID: [/]")
    eye_colors = Prompt.ask("[#71d5fb]Enter Eye ID: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            [{"IsOwned": False,
                "InventoryId": 0,
                "IsWearing": True,
                "ItemId": eye_id,
                "Colors": eye_colors,
                "Type": 1}]
        ]
    )
    if code == 500:
        console.print("FAILED | Unexpected Error", style="bold red")
    else:
        console.print("SUCCESS | Changed Eyes", style="bold green")


def wear_rareskin(server, ticket, actorId):
    skin_color = Prompt.ask("[#71d5fb]Enter skincolor: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            [{"IsOwned": False,
              "InventoryId": 0,
              "Type": 5,
              "ItemId": -1,
              "Colors": skin_color,
              "IsWearing": True}]
        ]
    )
    if code == 500:
        console.print("FAILED | Unexpected Error", style="bold red")
    else:
        console.print("SUCCESS | Changed Skincolor", style="bold green")

def add_to_wishlist(server, ticket):
    item_id = Prompt.ask("[#71d5fb]Enter ItemId: [/]")
    colors = Prompt.ask("[#71d5fb]Enter  colors: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Gifts.AMFGiftsService+Version2.AddItemToWishlist",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            [item_id],
            [colors]],
    )
    if code != 0:
        console.print(f"FAILED | {resp}", style="bold red")
    else:
        console.print("SUCCESS | item added to wishlist!", style="bold green")


def custom_status(server, ticket, actorId, name):
    status = Prompt.ask("[#71d5fb]Enter status: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.AMFActorService.SetMoodWithModerationCall",
        [ticketHeader(anyAttribute=None, ticket=ticket),
         {"Likes": 0, "TextLineBlacklisted": None, "WallPostLinks": None, "SpeechLine": False,
                         "FaceAnimation": "neutral", "WallPostId": 0, "ActorId": actorId, "IsBrag": False,
                         "FigureAnimation": "stand", "MouthAnimation": "none", "TextLineLastFiltered": None,
                         "TextLineWhitelisted": None, "TextLine": status}, name, 0, False],
    )
    if code == 500:
        console.print("FAILED | Unexpected Error", style="bold red")
    else:
        console.print("SUCCESS | status changed", style="bold green")



def recycle_items(server, ticket, actorId):
    relid = Prompt.ask("[#71d5fb]Enter itemid: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
        [ticketHeader(anyAttribute=None, ticket=ticket), actorId, relid,0]
    )
    if code == 500:
        console.print("FAILED | Unexpected Error", style="bold red")
    else:
        console.print("SUCCESS | Recycled item", style="bold green")



def wheel_spins(server, ticket, actorId):
    console.print("not implemented on alpha version")

def lisa_hack(server, ticket, actorId):
    console.print("not implemented on alpha version")

def automated_pixeller(server, ticket, actorId):
    console.print("not implemented on alpha version")

def msp_query(server, ticket, actorId):
    console.print("not implemented on alpha version")

def clothes_extractor(server, ticket):
    console.print("not implemented on alpha version")

def item_tracker(server):
    console.print("not implemented on alpha version")

def animations_extractor(server, ticket, actorId):
    console.print("not implemented on alpha version")

def bot_generator(server, culture):
    console.print("not implemented on alpha version")

def item_glitcher(server, ticket, actorId):
    console.print("not implemented on alpha version")

def automated_autographer(server, ticket, actorId):
    console.print("not implemented on alpha version")
