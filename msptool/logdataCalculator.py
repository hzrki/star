import hashlib

hash_bytes = bytearray()
sha1 = hashlib.sha1()
salt_characters_string = "Yd*xX#o@B15i@!th"
salt_characters_allowed_string = "UC4#Ti#DuwkJCov)!27Po#6d-FPzIET6kAmDqMsSK3^BKzhg0p+/Zaa4Z$iI+Xl2oHåpu+XA"
salt_characters_origin = None
salt_characters_allowed = None
salt_characters_allowed_copy = None
salt_numerics_origin = None
salt_numerics = None
salt_numerics_bytes = bytearray()

def get_log_data():
    global salt_characters_allowed, salt_characters_allowed_copy, salt_characters_origin, salt_numerics_origin, salt_numerics
    if salt_numerics_origin is None:
        salt_characters_allowed = list(salt_characters_allowed_string)
        salt_characters_allowed_copy = list(salt_characters_allowed_string)
        salt_characters_origin = list(salt_characters_string)
        salt_numerics_origin = []
        salt_numerics = [0] * 16

        for _loc3_ in salt_characters_origin:
            salt_numerics_origin.append(ord(_loc3_))

    for _loc2_ in range(len(salt_characters_origin)):
        _loc4_ = salt_numerics_origin[_loc2_]
        if _loc2_ % 2 == 0:
            _loc4_ += _loc2_
        else:
            _loc4_ -= _loc2_
        _loc5_ = _loc4_ % len(salt_characters_allowed_string)
        _loc6_ = salt_characters_allowed_copy[_loc5_]
        salt_numerics_bytes.extend(_loc6_.encode('utf-8'))
        swap_salt_indexes(_loc5_, salt_characters_allowed_copy)

    return salt_numerics_bytes.decode('utf-8')


def swap_salt_indexes(param1, param2):
    _loc3_ = len(param2)
    _loc4_ = (param1 + 14) % _loc3_
    _loc5_ = _loc4_ - 1 if _loc4_ - 1 >= 0 else _loc3_ - 1
    _loc6_ = _loc4_ + 1 if _loc4_ + 1 < _loc3_ else 0

    _loc7_ = param2[_loc5_]
    param2[_loc5_] = param2[_loc6_]
    param2[_loc6_] = _loc7_

print(get_log_data())