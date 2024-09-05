from rich.console import Console
from rich.prompt import Prompt
from amf import AmfCall
from checksum import ticketHeader
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

    if code == 500:
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

    if code == 500:
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
    total_amount = 0

    for _ in range(4):
        code, resp = AmfCall(server,
                             "MovieStarPlanet.WebService.Awarding.AMFAwardingService.claimDailyAward",
                             [ticketHeader(anyAttribute=None, ticket=ticket),"starwheel",120, actorId]
                                    )
        if code == 200:
            amount = resp.get('Content', {}).get('amount', {})
            if isinstance(amount, int):
                total_amount += amount

    for _ in range(4):
        code, resp = AmfCall(server,
                                    "MovieStarPlanet.WebService.Awarding.AMFAwardingService.claimDailyAward",
                                    [ticketHeader(anyAttribute=None, ticket=ticket), "starVipWheel", 200, actorId]
                                    )
        if code == 200:
            amount = resp.get('Content', {}).get('amount', {})
            if isinstance(amount, int):
                total_amount += amount

    for _ in range(2):
        code, resp = AmfCall(server,
                                    "MovieStarPlanet.WebService.Awarding.AMFAwardingService.claimDailyAward",
                                    [ticketHeader(anyAttribute=None, ticket=ticket), "advertWheelDwl", 240, actorId]
                                    )
        if code == 200:
            amount = resp.get('Content', {}).get('amount', {})
            if isinstance(amount, int):
                total_amount += amount

    for _ in range(2):
        code, resp = AmfCall(server,
                                    "MovieStarPlanet.WebService.Awarding.AMFAwardingService.claimDailyAward",
                                    [ticketHeader(anyAttribute=None, ticket=ticket), "advertWheelVipDwl", 400, actorId]
                                    )
        if code == 200:
            amount = resp.get('Content', {}).get('amount', {})
            if isinstance(amount, int):
                total_amount += amount

                console.print(f"SUCCESS | {total_amount} starcoins have been added to your account", style="bold green")


def msp_query(server, ticket, actorId):
    qusername = Prompt.ask("[#71d5fb]Enter username: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
        [qusername]
    )

    if resp == -1:
        console.print("FAILED | The account doesn't exist or has been deleted", style="bold red")
        return

    if code != 200:
        console.print("FAILED | Unexpected Error", style="bold red")
        return

    if isinstance(resp, int):
        actor_id = resp
    else:
        actor_id = resp.get("Content")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Profile.AMFProfileService.LoadProfileSummary",
        [ticketHeader(anyAttribute=None, ticket=ticket), actor_id, actorId]
    )
    createdate = resp.get('Created')

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.AMFActorService.LoadMovieStarListRevised",
        [ticketHeader(anyAttribute=None, ticket=ticket), [actor_id]]
    )

    if code != 200:
        console.print("FAILED | Unexpected Error", style="bold red")
        return

    actor_info = resp[0]

    nebulaProfileId = actor_info['NebulaProfileId']
    actorId2 = actor_info['ActorId']
    username = actor_info['Name']
    level = actor_info['Level']
    fame = int(actor_info['Fame'])
    starcoins = actor_info['Money']
    diamonds = actor_info['Diamonds']
    skinColor = actor_info['SkinColor']
    eyeId = actor_info['EyeId']
    eyeColors = actor_info['EyeColors']
    noseId = actor_info['NoseId']
    mouthId = actor_info['MouthId']
    mouthColors = actor_info['MouthColors']
    membershiptimeoutdate = actor_info['MembershipTimeoutDate']
    LastLogin = actor_info['LastLogin']

    md = membershiptimeoutdate.strftime("%Y-%m-%d %H:%M:%S")

    llg = createdate.strftime("%Y-%m-%d %H:%M:%S")

    console.print(f"{username}'s Information", style="bold red")
    console.print(f"ActorId: {actorId2}", style="bold red")
    console.print(f"NebulaProfileId: {nebulaProfileId}", style="bold red")
    console.print(f"Username: {username}", style="bold red")
    console.print(f"Level: {level}", style="bold red")
    console.print(f"Fame: {fame}", style="bold red")
    console.print(f"Starcoins: {starcoins}", style="bold red")
    console.print(f"Diamonds: {diamonds}", style="bold red")
    console.print(f"SkinColor: {skinColor}", style="bold red")
    console.print(f"EyeId: {eyeId}", style="bold red")
    console.print(f"EyeColors: {eyeColors}", style="bold red")
    console.print(f"NoseId: {noseId}", style="bold red")
    console.print(f"MouthId: {mouthId}", style="bold red")
    console.print(f"MouthColors: {mouthColors}", style="bold red")
    console.print(f"Created: {llg}", style="bold red")
    console.print(f"MembershipTimeoutDate: {md}", style="bold red")
    console.print(f"LastLogin: {LastLogin}", style="bold red")


