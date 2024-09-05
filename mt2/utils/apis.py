from PyQt5 import QtWidgets, QtCore
from utils.amf import AmfCall
from utils.checksum import ticketHeader

def buy_boonie(server, ticket, actorId, dialog):
    boonie_id, ok = QtWidgets.QInputDialog.getText(dialog, "Buy Boonie", "Enter Boonie ID:")
    if ok:
        code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Pets.AMFPetService.BuyClickItem",
            [
                ticketHeader(anyAttribute=None, ticket=ticket),
                actorId,
                int(boonie_id)
            ],
        )
        if code == 500:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | BoonieId not found")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Boonie bought!")

def buy_animation(server, ticket, actorId, dialog):
    animation_id, ok = QtWidgets.QInputDialog.getText(dialog, "Buy Animation", "Enter Animation ID:")
    if ok:
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | AnimationId was not found")
        elif description:
            QtWidgets.QMessageBox.critical(dialog, "Error", f"FAILED | {description}")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Animation bought!")

def buy_clothes(server, ticket, actorId, dialog):
    rare_id, ok1 = QtWidgets.QInputDialog.getText(dialog, "Buy Clothes", "Enter Clothes ID:")
    color, ok2 = QtWidgets.QInputDialog.getText(dialog, "Buy Clothes", "Enter Color:")
    if ok1 and ok2:
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Not allowed to spawn item")
        elif description:
            QtWidgets.QMessageBox.critical(dialog, "Error", f"FAILED | {description}")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Clothing item bought!")

def buy_eyes(server, ticket, actorId, dialog):
    eye_id, ok1 = QtWidgets.QInputDialog.getText(dialog, "Buy Eyes", "Enter Eye ID:")
    eye_colors, ok2 = QtWidgets.QInputDialog.getText(dialog, "Buy Eyes", "Enter Eye Colors:")
    if ok1 and ok2:
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Changed Eyes")

def wear_rareskin(server, ticket, actorId, dialog):
    skin_color, ok = QtWidgets.QInputDialog.getText(dialog, "Wear Rare Skin", "Enter skin color:")
    if ok:
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Changed Skincolor")

def add_to_wishlist(server, ticket, dialog):
    item_id, ok1 = QtWidgets.QInputDialog.getText(dialog, "Add to Wishlist", "Enter Item ID:")
    colors, ok2 = QtWidgets.QInputDialog.getText(dialog, "Add to Wishlist", "Enter Colors:")
    if ok1 and ok2:
        code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Gifts.AMFGiftsService+Version2.AddItemToWishlist",
            [
                ticketHeader(ticket=ticket,anyAttribute=None),
                [int(item_id)],
                [colors]],
        )

        if resp == 0:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Item added to wishlist!")
        elif resp == -1:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Not allowed to add item to wishlist!")
        elif resp == 12:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Wishlist is full!")

def custom_status(server, ticket, actorId, name, dialog):
    status, ok = QtWidgets.QInputDialog.getText(dialog, "Custom Status", "Enter status:")
    if ok:
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Status changed")

def recycle_items(server, ticket, actorId, dialog):
    relid, ok = QtWidgets.QInputDialog.getText(dialog, "Recycle Items", "Enter item ID:")
    if ok:
        code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
            [ticketHeader(anyAttribute=None, ticket=ticket), actorId, int(relid),0]
        )
        if code == 500:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
        else:
            QtWidgets.QMessageBox.information(dialog, "Success", "SUCCESS | Recycled item")

def wheel_spins(server, ticket, actorId, dialog):
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

    QtWidgets.QMessageBox.information(dialog, "Success", f"{total_amount} starcoins have been added to your account")

def msp_query(server, ticket, actorId, dialog):
    qusername, ok = QtWidgets.QInputDialog.getText(dialog, "Query", "Enter username:")
    if ok:
        code, resp = AmfCall(
            server,
            "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
            [qusername]
        )

        if resp == -1:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | The account doesn't exist or has been deleted")
            return

        if code != 200:
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
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
            QtWidgets.QMessageBox.critical(dialog, "Error", "FAILED | Unexpected Error")
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

        QtWidgets.QMessageBox.information(dialog, "Account Information", info_message)
