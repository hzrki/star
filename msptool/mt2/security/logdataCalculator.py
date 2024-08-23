import hashlib

hashBytes = bytearray()
sha1 = hashlib.sha1()
saltCharactersString = "Yd*xX#o@B15i@!th"
saltCharactersAllowedString = "UC4#Ti#DuwkJCov)!27Po#6d-FPzIET6kAmDqMsSK3^BKzhg0p+/Zaa4Z$iI+Xl2oHåpu+XA"
saltCharactersOrigin = None
saltCharactersAllowed = None
saltCharactersAllowedCopy = None
saltNumericsOrigin = None
saltNumerics = None
saltNumericsBytes = bytearray()

def getLogData():
    global saltCharactersAllowed, saltCharactersAllowedCopy, saltCharactersOrigin, saltNumericsOrigin, saltNumerics
    if saltNumericsOrigin is None:
        saltCharactersAllowed = list(saltCharactersAllowedString)
        saltCharactersAllowedCopy = list(saltCharactersAllowedString)
        saltCharactersOrigin = list(saltCharactersString)
        saltNumericsOrigin = []
        saltNumerics = [0] * 16

        for loc3 in saltCharactersOrigin:
            saltNumericsOrigin.append(ord(loc3))

    for loc2 in range(len(saltCharactersOrigin)):
        loc4 = saltNumericsOrigin[loc2]
        if loc2 % 2 == 0:
            loc4 += loc2
        else:
            loc4 -= loc2
        loc5 = loc4 % len(saltCharactersAllowedString)
        loc6 = saltCharactersAllowedCopy[loc5]
        saltNumericsBytes.extend(loc6.encode('utf-8'))
        swapSaltIndexes(loc5, saltCharactersAllowedCopy)

    return saltNumericsBytes.decode('utf-8')


def swapSaltIndexes(param1, param2):
    loc3 = len(param2)
    loc4 = (param1 + 14) % loc3
    loc5 = loc4 - 1 if loc4 - 1 >= 0 else loc3 - 1
    loc6 = loc4 + 1 if loc4 + 1 < loc3 else 0

    loc7 = param2[loc5]
    param2[loc5] = param2[loc6]
    param2[loc6] = loc7

print(getLogData())