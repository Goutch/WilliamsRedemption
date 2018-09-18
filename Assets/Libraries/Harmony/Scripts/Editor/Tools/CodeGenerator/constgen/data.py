import hashlib


def _hash(name):
    return int(hashlib.md5(name.encode('utf-8')).hexdigest(), 16) % (10 ** 8)


class ConstClass:
    def __init__(self, name, members):
        self.id = _hash(name)
        self.name = name
        self.members = members


class ConstMember:
    def __init__(self, name, path):
        self.id = _hash(name)
        self.name = name
        self.value = name
        self.path = path
        self.isValid = name.isalnum() and name != "GameObject" and name != "Scene" and name != "Prefab" and name != "Layer" and name != "Tag" and name != "AnimatorParameter"
