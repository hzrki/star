import tkinter as tk
from tkinter import ttk
import pymem

def ijv():
    try:
        pm = pymem.Pymem("ml.exe")
        loc1 = anidd.get()

        if loc1 == "Rifle Hunter":
            loc2 = b"Layingonside"
            loc3 = b"Rifle Hunter"
        elif loc1 == "Blood on the Floor":
            loc2 = b"cloud_2018_smallharp_mf"
            loc3 = b"blood_on_the_dancefloor"
        elif loc1 == "Chainsaw":
            loc2 = b"callme00"
            loc3 = b"chainsaw"
        elif loc1 == "Bazooka":
            loc2 = b"teacher"
            loc3 = b"bazooka"
        elif loc1 == "Double Fuck":
            loc2 = b"zombiewalk"
            loc3 = b"double_fuck"
        elif loc1 == "Meat Cleaver":
            loc2 = b"fashinweek_2012_fallover2_mf"
            loc3 = b"halloween_2011_meatcleaver_mf"
        elif loc1 == "Gun Pose":
            loc2 = b"rock_on"
            loc3 = b"gunpose"
        elif loc1 == "Gun From Purse":
            loc2 = b"magic carpet"
            loc3 = b"gunfrompurse"
        elif loc1 == "Throw Grenade":
            loc2 = b"knockonscren"
            loc3 = b"throw grende"
        elif loc1 == "Faar hugget hovedet":
            loc2 = b"jetse_2012_trolley_mf"
            loc3 = b"faar_hugget_hovedet_af"
        elif loc1 == "Kravler":
            loc2 = b"teacher"
            loc3 = b"kravler"
        elif loc1 == "tank modern":
            loc2 = b"kickoff_2012_bootit_mf"
            loc3 = b"acion_2011_tankmodern"
        elif loc1 == "kroseksplotion":
            loc2 = b"superhero_hover"
            loc3 = b"kropseksplotion"
        else:
            loc2 = b""
            loc3 = b""

        if loc2 and loc3:
            for loc4 in pymem.pattern.pattern_scan_all(pm.process_handle, loc2, return_multiple=True):
                pm.write_bytes(loc4, loc3, len(loc3))
            rslt.config(state=tk.NORMAL)
            rslt.delete("1.0", tk.END)
            rslt.insert(tk.END, f"{loc3.decode()} has been added to your animations")
            rslt.config(state=tk.DISABLED)
        else:
            rslt.config(state=tk.NORMAL)
            rslt.delete("1.0", tk.END)
            rslt.insert(tk.END, "No animation selected")
            rslt.config(state=tk.DISABLED)

    except Exception as e:
        rslt.config(state=tk.NORMAL)
        rslt.delete("1.0", tk.END)
        rslt.insert(tk.END, f"ERROR!")
        rslt.config(state=tk.DISABLED)

def cbu():
    try:
        pm = pymem.Pymem("ml.exe")
        loc1 = b'\x75\x6E\x70\x65\x72\x6D\x69\x74\x74\x65\x64'
        loc2 = b'\x70\x65\x72\x6D\x69\x74\x74\x65\x64'

        for loc3 in pymem.pattern.pattern_scan_all(pm.process_handle, loc1, return_multiple=True):
            pm.write_bytes(loc3, loc2, len(loc2))

        rslt.config(state=tk.NORMAL)
        rslt.delete("1.0", tk.END)
        rslt.insert(tk.END, "Chatban has been unlocked")
        rslt.config(state=tk.DISABLED)

    except Exception as e:
        rslt.config(state=tk.NORMAL)
        rslt.delete("1.0", tk.END)
        rslt.insert(tk.END, f"ERROR!")
        rslt.config(state=tk.DISABLED)

def sla(*args):
    loc1 = anidd.get()

hasumem = tk.Tk()
hasumem.title("Hasu Mem")
hasumem.geometry("500x280")
hasumem.resizable(False, False)

sye = ttk.Style()
sye.configure('TLabel', font=('Arial', 12))
sye.configure('TButton', font=('Arial', 12))
sye.configure('TEntry', font=('Arial', 12))
sye.configure('TMenubutton', font=('Arial', 12), borderwidth=2, relief="groove")

mf1 = ttk.Frame(hasumem)
mf1.grid(row=0, column=0, padx=20, pady=20)

al1 = ttk.Label(mf1, text="Banned animations:")
al1.grid(row=0, column=0, sticky="w")

aniops = [
    "Select an Animation", "Rifle Hunter", "Blood on the Floor", "Chainsaw", "Bazooka",
    "Double Fuck", "Meat Cleaver", "Gun Pose", "Gun From Purse", "Throw Grenade",
    "Faar hugget hovedet", "Kravler", "tank modern", "kroseksplotion"
]
anidd = tk.StringVar()
anidd.set(aniops[0])
aniddm = ttk.OptionMenu(mf1, anidd, *aniops)
aniddm.grid(row=0, column=1, padx=5, pady=5)

anidd.trace("w", sla)

ijb = ttk.Button(mf1, text="Inject", command=ijv)
ijb.grid(row=0, column=2, padx=5, pady=5)

rslt = tk.Text(mf1, height=10, width=50, state=tk.DISABLED)
rslt.grid(row=1, column=0, columnspan=3, padx=5, pady=5)

cub = ttk.Button(mf1, text="ChatBan Unlocker", command=cbu)
cub.grid(row=2, column=2, padx=5, pady=5)

hasumem.mainloop()
