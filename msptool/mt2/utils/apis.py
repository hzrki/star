from rich.console import Console
from rich.prompt import Prompt
from utils.amf import AmfCall
from utils.checksum import ticketHeader
console = Console()
import time
import pymem

def buy_boonie(server, ticket, actorId):
    with console.status("[#71d5fb]Loading BoonieIds...[/]", spinner="star") as status:
        time.sleep(5)

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Pets.AMFPetService.GetClickItems",
        [ticketHeader(anyAttribute=None, ticket=ticket)],
    )
    for boonies in range(len(resp)):
        ia = resp[boonies]
        boonie_ids = ia.get("ClickItemId")
        boonie_names = ia.get("Name")
        price = ia.get("Price")
        console.print(f"[#71d5fb]BoonieId: {boonie_ids} | Name: {boonie_names} | Price: {price}")

    boonie_id = Prompt.ask("[#71d5fb]Enter Boonie ID: [/]")

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Pets.AMFPetService.BuyClickItem",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
            int(boonie_id)
        ],
    )

    code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Pets.AMFPetService.GetClickItemsForActor",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            actorId,
        ],
    )
    actorBoonieRelid = resp[-1]["ActorClickItemRelId"]

    for _ in range(5):
        code, resp = AmfCall(
        server,
        "MovieStarPlanet.WebService.Pets.AMFPetService.FeedPet",
        [
            ticketHeader(anyAttribute=None, ticket=ticket),
            int(actorBoonieRelid),
            2
        ],
    )

    if code != 200:
        console.print("FAILED | BoonieId not found", style="bold red")
    else:
        console.print("SUCCESS | Boonie bought!", style="bold green")

def buy_animation(server, ticket, actorId):
    animation_id = Prompt.ask("[#71d5fb]Animation ID: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
            [
                ticketHeader(anyAttribute=None, ticket=ticket),
                actorId,
                int(animation_id)
            ],
        )
    description = resp.get('Description', '')
    if code == 500:
        console.print("FAILED | AnimationId was not found")
    elif description:
        console.print(f"FAILED | {description}")
    else:
        console.print("SUCCESS | Animation bought!")

def buy_clothes(server, ticket, actorId):
    rare_id = Prompt.ask("[#71d5fb]Clothes ID: [/]")
    color = Prompt.ask("[#71d5fb]Colors: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.AMFSpendingService.BuyClothes",
            [
                ticketHeader(anyAttribute=None, ticket=ticket),
                actorId,
                [{"Color": color,
                  "x": 0,
                  "ClothesId": int(rare_id),
                  "ActorClothesRelId": 0,
                  "y": 0,
                  "IsWearing": 1,
                 "ActorId": actorId}],
                0
            ],
        )
    description = resp.get('Description', '')

    if code != 200:
        console.print("FAILED | Not allowed to spawn item")
    elif description:
        console.print(f"FAILED | {description}")
    else:
        console.print("SUCCESS | Clothing item bought!")

def buy_eyes(server, ticket, actorId):
    eye_id = Prompt.ask("[#71d5fb]Eye ID: [/]")
    eye_colors = Prompt.ask("[#71d5fb]Eye Colors: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
            [
                ticketHeader(anyAttribute=None, ticket=ticket),
                actorId,
                [{"IsOwned": False,
                    "InventoryId": 0,
                    "IsWearing": True,
                    "ItemId": int(eye_id),
                    "Colors": eye_colors,
                    "Type": 1}]
            ]
        )
    if code == 500:
        console.print("FAILED | Unexpected Error")
    else:
        console.print("SUCCESS | Changed Eyes")

def wear_rareskin(server, ticket, actorId):
    skin_color = Prompt.ask("[#71d5fb]skincolor: [/]")
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
        console.print("FAILED | Unexpected Error")
    else:
        console.print("SUCCESS | Changed Skincolor")

def add_to_wishlist(server, ticket):
    item_id = Prompt.ask("[#71d5fb]Item ID: [/]")
    colors = Prompt.ask("[#71d5fb]Colors: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Gifts.AMFGiftsService+Version2.AddItemToWishlist",
            [
                ticketHeader(ticket=ticket,anyAttribute=None),
                [int(item_id)],
                [colors]],
        )

    if resp == 0:
        console.print("SUCCESS | Item added to wishlist!")
    elif resp == -1:
        console.print("FAILED | Not allowed to add item to wishlist!")
    elif resp == 12:
        console.print("FAILED | Wishlist is full!")

def custom_status(server, ticket, actorId, name):
    status = Prompt.ask("[#71d5fb]status: [/]")
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
        console.print("FAILED | Unexpected Error")
    else:
        console.print("SUCCESS | Status changed")

def recycle_items(server, ticket, actorId):
    relid = Prompt.ask("[#71d5fb]item ID: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
            [ticketHeader(anyAttribute=None, ticket=ticket), actorId, int(relid),0]
        )
    if code == 500:
        console.print("FAILED | Unexpected Error")
    else:
        console.print("SUCCESS | Recycled item")

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

    console.print(f"SUCCESS | {total_amount} starcoins have been added to your account")

def msp_query(server, ticket, actorId):
    qusername = Prompt.ask("[#71d5fb]username: [/]")
    code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
            [qusername]
        )

    if resp == -1:
        console.print("FAILED | The account doesn't exist or has been deleted")
        return

    if code != 200:
        console.print("FAILED | Unexpected Error")
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
        console.print("FAILED | Unexpected Error")
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

    info_message = (f"{username}'s Information\n"
                        f"ActorId: {actorId2}\n"
                        f"NebulaProfileId: {nebulaProfileId}\n"
                        f"Username: {username}\n"
                        f"Level: {level}\n"
                        f"Fame: {fame}\n"
                        f"Starcoins: {starcoins}\n"
                        f"Diamonds: {diamonds}\n"
                        f"SkinColor: {skinColor}\n"
                        f"EyeId: {eyeId}\n"
                        f"EyeColors: {eyeColors}\n"
                        f"NoseId: {noseId}\n"
                        f"MouthId: {mouthId}\n"
                        f"MouthColors: {mouthColors}\n"
                        f"Created: {llg}\n"
                        f"MembershipTimeoutDate: {md}\n"
                        f"LastLogin: {LastLogin}")

    console.print("Account Information", info_message)


def lisa(server, ticket, actorId):
    total_amount_sc = 0
    total_amount_fame = 0
    wheel_spins(server, ticket, actorId)

    for i in range(100):
        if i % 2 == 0:
            code, resp = AmfCall(
                server,
                "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward",
                [ticketHeader(anyAttribute=None, ticket=ticket), "twoPlayerMoney", 50, actorId],
            )

            if code == 500:
                console.print("[FAILED] :(", style="bold red")
                continue

            if resp.get("amount") == 50:
                total_amount_sc += 50
                console.print("[SUCCESS] 50 SC added :)", style="bold green")
            elif resp.get("amount") == -1:
                break

        else:
            code, resp = AmfCall(
                server,
                "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward",
                [ticketHeader(anyAttribute=None, ticket=ticket), "twoPlayerFame", 50, actorId],
            )

            if code == 500:
                console.print("[FAILED] :(", style="bold red")
                continue

            if resp.get("amount") == 50:
                total_amount_fame += 50
                console.print("[SUCCESS] 50 Fame added :)", style="bold green")
            elif resp.get("amount") == -1:
                break

        time.sleep(1)

        code, resp = AmfCall(
                server,
                "MovieStarPlanet.WebService.Achievement.AMFAchievementWebService.ClaimReward",
                [ticketHeader(anyAttribute=None, ticket=ticket),"LUCKY_YOU", actorId],
            )

    return total_amount_sc, total_amount_fame

def chatban_unlocker():
    try:
        pm = pymem.Pymem("ml.exe")
        loc1 = b'\x75\x6E\x70\x65\x72\x6D\x69\x74\x74\x65\x64'
        loc2 = b'\x70\x65\x72\x6D\x69\x74\x74\x65\x64'

        for loc3 in pymem.pattern.pattern_scan_all(pm.process_handle, loc1, return_multiple=True):
            pm.write_bytes(loc3, loc2, len(loc2))

        console.print("Chatban has been unlocked", style="bold green")

    except Exception as e:
        console.print("ERROR!", style="bold red")

def banned_animations():
        banimations = {
            1: "Rifle Hunter",
            2: "Blood on the Floor",
            3: "Chainsaw",
            4: "Bazooka",
            5: "Double Fuck",
            6: "Meat Cleaver",
            7: "Gun Pose",
            8: "Gun From Purse",
            9: "Throw Grenade",
            10: "Faar hugget hovedet",
            11: "Kravler",
            12: "tank modern",
            13: "kroseksplotion"
        }

        console.print("Select an animation by number:")
        for num, animation in banimations.items():
            console.print(f"[bold cyan]{num}: {animation}[/]")

        try:
            choice = int(Prompt.ask("Enter animation to soft: "))
            loc1 = banimations.get(choice)

            if not loc1:
                console.print("Please type an existing animation : )", style="bold red")
                return
            animation_map = {
                "Rifle Hunter": (b"Layingonside", b"Rifle Hunter"),
                "Blood on the Floor": (b"cloud_2018_smallharp_mf", b"blood_on_the_dancefloor"),
                "Chainsaw": (b"callme00", b"chainsaw"),
                "Bazooka": (b"teacher", b"bazooka"),
                "Double Fuck": (b"zombiewalk", b"double_fuck"),
                "Meat Cleaver": (b"fashinweek_2012_fallover2_mf", b"halloween_2011_meatcleaver_mf"),
                "Gun Pose": (b"rock_on", b"gunpose"),
                "Gun From Purse": (b"magic carpet", b"gunfrompurse"),
                "Throw Grenade": (b"knockonscren", b"throw grende"),
                "Faar hugget hovedet": (b"jetse_2012_trolley_mf", b"faar_hugget_hovedet_af"),
                "Kravler": (b"teacher", b"kravler"),
                "tank modern": (b"kickoff_2012_bootit_mf", b"acion_2011_tankmodern"),
                "kroseksplotion": (b"superhero_hover", b"kropseksplotion")
            }

            loc2, loc3 = animation_map.get(loc1, (b"", b""))

            pm = pymem.Pymem("ml.exe")

            if loc2 and loc3:
                for loc4 in pymem.pattern.pattern_scan_all(pm.process_handle, loc2, return_multiple=True):
                    pm.write_bytes(loc4, loc3, len(loc3))
                console.print(f"[bold green]{loc3.decode()} has been added to your animations[/]", style="bold green")
            else:
                console.print("ERROR!", style="bold red")

        except ValueError:
            console.print("Please type an existing animation : )", style="bold red")