import os

from dpath.util import values as searchYamlContent
from yaml import load_all as readYamlFile


def _isNotNone(object):
    return object is not None


def _hasExtension(object, extension):
    return object.endswith(extension)


class Item:
    def __init__(self, value, path):
        self.value = value
        self.path = path

    def __lt__(self, other):
        return self.value < other.value

    def __eq__(self, other):
        if isinstance(self, other.__class__):
            return self.value == other.value
        return False

    def __hash__(self):
        return self.value.__hash__()


class Folder:
    def __init__(self, path, search):
        self.path = path
        self.__content = self.__readFolder(path, search)

    def readConsts(self):
        return self.__content

    def __readFolder(self, path, search):
        items = []
        for root, subdirs, files in os.walk(path):
            for name in files:
                if _hasExtension(name, search):
                    items.append(Item(name.replace(search, ""), os.path.join(root, name)))
        return list(filter(_isNotNone, items))


class File:
    def __init__(self, path, search):
        self.path = path
        self.__content = self.__readFile(path, search)

    def readConsts(self):
        return self.__content

    def __readFile(self, path, search):
        items = ""
        with open(path, 'r') as fileStream:
            for line in fileStream.readlines():
                if line.startswith('--- !u!'):
                    items += '--- ' + line.split(' ')[2] + '\n'
                else:
                    items += line
        return list(map(lambda item: Item(item, path), filter(_isNotNone, searchYamlContent(list(readYamlFile(items)), search))))