import sys
import os
import zipfile
from io import BytesIO
from curl_cffi import requests
from PyQt5 import QtWidgets, QtGui, QtCore
from utils.amf import AmfCall
from utils.apis import buy_boonie, buy_animation, buy_clothes, buy_eyes, wear_rareskin, add_to_wishlist, custom_status, \
    recycle_items, wheel_spins, msp_query
from utils.settings import WebServer, Loc1, load_avatar, check_version


def assets_exists():
    loc1 = './assets'
    if not os.path.exists(loc1):
        print("DEV | Assets folder not found. Downloading...")
        loc2 = "https://github.com/r-h-y/star/archive/refs/heads/main.zip"
        resp = requests.get(loc2)
        if resp.status_code == 200:
            with zipfile.ZipFile(BytesIO(resp.content)) as z:
                loc3 = 'temp_assets'
                z.extractall(loc3)
                os.rename(os.path.join(loc3, 'star-main/mt2/assets'), loc1)
                print("Assets downloaded and extracted successfully.")
                for root, dirs, files in os.walk(loc3, topdown=False):
                    for name in files:
                        os.remove(os.path.join(root, name))
                    for name in dirs:
                        os.rmdir(os.path.join(root, name))
                os.rmdir(loc3)
        else:
            print("DEV |Unexpected error")
    else:
        print("DEV |Assets folder already exists.")


def perform_login(server, username, password, status_label, window, layout):
    status_label.setText("Logging in...")
    code, resp = AmfCall(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                         [username, password, [], None, None, "MSP1-Standalone:XXXXXX"])

    logged_in = resp.get('loginStatus', {}).get('status')
    locked_status = resp.get('loginStatus', {}).get('statusDetails')

    if logged_in == "InvalidCredentials":
        status_label.setText("Invalid username or password")
    elif locked_status == "LockPermanent":
        status_label.setText("Your account is permanently locked")
    elif logged_in == "Success":
        status_label.setText(f"Success! Logged in as {username}")
        actorId = resp['loginStatus']['actor']['ActorId']
        name = resp["loginStatus"]["actor"]["Name"]
        ticket = resp['loginStatus']['ticket']
        accessToken = resp["loginStatus"]["nebulaLoginStatus"]["accessToken"]
        profileId = resp["loginStatus"]["nebulaLoginStatus"]["profileId"]

        homeMenu(window, layout, server, ticket, name, actorId, accessToken, profileId)
    else:
        status_label.setText("Login failed. Please try again.")


def homeMenu(window, layt_, server, ticket, name, actorId, accessToken, profileId):
    for i in reversed(range(layt_.count())):
        home_widget = layt_.itemAt(i).widget()
        if home_widget is not None:
            home_widget.deleteLater()

    home_ui = QtWidgets.QHBoxLayout()
    layt_.addLayout(home_ui)

    avatar_layout = QtWidgets.QVBoxLayout()
    home_ui.addLayout(avatar_layout)

    avatar_label = QtWidgets.QLabel(window)
    avatar_url = f"https://snapshots.mspcdns.com/v1/MSP/{server}/snapshot/fullSizeMoviestar/{actorId}.jpg"

    load_avatar(avatar_url, avatar_label)
    avatar_layout.addWidget(avatar_label)

    name_label = QtWidgets.QLabel(name, window)
    name_label.setAlignment(QtCore.Qt.AlignCenter)
    name_label.setStyleSheet("color: white;")
    avatar_layout.addWidget(name_label)

    btn_layout = QtWidgets.QGridLayout()
    home_ui.addLayout(btn_layout)

    options = [
        ("Buy Boonie", "./assets/boonie_icon.png", lambda: buy_boonie(server, ticket, actorId, window)),
        ("Buy Animation", "./assets/animation_icon.png", lambda: buy_animation(server, ticket, actorId, window)),
        ("Buy Clothes", "./assets/clothes_icon.png", lambda: buy_clothes(server, ticket, actorId, window)),
        ("Buy Eyes", "./assets/eyes_icon.png", lambda: buy_eyes(server, ticket, actorId, window)),
        ("Wear Rare Skin", "./assets/skin_icon.png", lambda: wear_rareskin(server, ticket, actorId, window)),
        ("Add to Wishlist", "./assets/wishlist_icon.png", lambda: add_to_wishlist(server, ticket, window)),
        ("Custom Status", "./assets/status_icon.png", lambda: custom_status(server, ticket, actorId, name, window)),
        ("Recycle Items", "./assets/recycle_icon.png", lambda: recycle_items(server, ticket, actorId, window)),
        ("Wheel Spins", "./assets/wheel_icon.png", lambda: wheel_spins(server, ticket, actorId, window)),
        ("Query MSP", "./assets/query_icon.png", lambda: msp_query(server, ticket, actorId, window))
    ]

    for index, (option, icon_path, callback) in enumerate(options):
        btn = QtWidgets.QPushButton(window)
        btn.setIcon(QtGui.QIcon(icon_path))
        btn.setIconSize(QtCore.QSize(80, 80))
        btn.clicked.connect(callback)

        row = index // 2
        col = index % 2
        btn_layout.addWidget(btn, row, col)

    logout_btn = QtWidgets.QPushButton("Logout", window)
    logout_btn.clicked.connect(lambda: _init_(window, layt_))
    layt_.addWidget(logout_btn)


def login(window, layt_, status_label, srv_dd, usr_, pw_):
    check_version(status_label)
    username = usr_.text()
    password = pw_.text()
    selected_server = WebServer(srv_dd.currentIndex() + 1)
    server = Loc1.loc3(selected_server)[1]

    status_label.setText("Logging in...")
    QtCore.QTimer.singleShot(1500, lambda: perform_login(server, username, password, status_label, window, layt_))


def _init_(window, layt_):
    for i in reversed(range(layt_.count())):
        home_widget = layt_.itemAt(i).widget()
        if home_widget is not None:
            home_widget.deleteLater()

    loc1 = QtGui.QPalette()
    loc2 = QtGui.QPixmap("./assets/mt2_bg.png")
    loc1.setBrush(QtGui.QPalette.Background, QtGui.QBrush(loc2))
    central_widget.setPalette(loc1)
    central_widget.setAutoFillBackground(True)

    title_lbl = QtWidgets.QLabel("MT2", window)
    title_lbl.setFont(QtGui.QFont("Arial", 16))
    title_lbl.setAlignment(QtCore.Qt.AlignCenter)
    title_lbl.setStyleSheet("color: white;")
    layt_.addWidget(title_lbl)

    usr_ = QtWidgets.QLineEdit(window)
    usr_.setPlaceholderText("Enter username")
    layt_.addWidget(usr_)

    pw_ = QtWidgets.QLineEdit(window)
    pw_.setPlaceholderText("Enter password")
    pw_.setEchoMode(QtWidgets.QLineEdit.Password)
    layt_.addWidget(pw_)

    srv_dd = QtWidgets.QComboBox(window)
    srv_dd.addItems([Loc1.loc3(ws)[0] for ws in WebServer])
    layt_.addWidget(srv_dd)

    login_btn = QtWidgets.QPushButton("Login", window)
    login_btn.clicked.connect(
        lambda: login(window, layt_, status_label, srv_dd, usr_, pw_))
    layt_.addWidget(login_btn)

    status_label = QtWidgets.QLabel("", window)
    status_label.setStyleSheet("color: white;")
    layt_.addWidget(status_label)


if __name__ == "__main__":
    assets_exists()
    app = QtWidgets.QApplication(sys.argv)
    window = QtWidgets.QMainWindow()
    window.setWindowTitle("MT2")
    window.setFixedSize(400, 500)
    central_widget = QtWidgets.QWidget(window)
    window.setCentralWidget(central_widget)
    layt_ = QtWidgets.QVBoxLayout(central_widget)
    _init_(window, layt_)
    window.show()
    sys.exit(app.exec_())
