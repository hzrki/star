import hashlib
import random
import binascii
from pyamf import ASObject


def getMarkingId() -> str:
    loc1 = str(random.randint(0, 999))
    loc2 = hashlib.md5(loc1.encode()).hexdigest()
    return f"{loc2}{loc1}"


def ticketHeader(anyAttribute: str = None, ticket: str = "") -> ASObject:
    loc1 = getMarkingId().encode()
    loc2 = hashlib.md5(loc1).hexdigest()
    loc3 = binascii.hexlify(loc1).decode()
    loc4 = f"{ticket}{loc2}{loc3}"
    return ASObject({"anyAttribute": anyAttribute, "Ticket": loc4})
