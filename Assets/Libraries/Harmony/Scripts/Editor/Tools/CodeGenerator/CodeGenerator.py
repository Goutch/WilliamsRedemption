import datetime
import os
import sys

from constgen.data import ConstClass, ConstMember
from constgen.source import Item, Folder, File
from mako.template import Template

"""
FACTORIES
"""


def createConstClass(name, members):
    return ConstClass(name, members)


def createConstMembers(items):
    return list(map(lambda item: ConstMember(item.value, item.path), sorted(set(items))))


"""
MAIN PROGRAM
"""

if len(sys.argv) != 3 or "-h" in sys.argv:
    print("""Generate a class of usefull consts using a Unity Project. Generates consts for Layers, Tags, Scenes, Prefabs, GameObjects and Animator Parameters.

             Usage :            
                 HarmonyCodeGenerator.py inputDir outputDir
             Where :
                 inputDir                Path to the Unity project directory (not the Asset folder).
                 outputDir               Path to the output directory for generated code (in the Asset folder).""")
else:

    # Arguments
    projectDirectoryPath = sys.argv[1]
    outputDirectoryPath = sys.argv[2]

    print("Creating (if not exist) output folder.")

    # Make directories
    if not os.path.exists(outputDirectoryPath):
        os.makedirs(outputDirectoryPath)

    print("")
    print("Reading files.")

    # Template
    constClasses = []
    template = Template(filename=os.path.join(os.path.dirname(os.path.realpath(__file__)), "R.cs.mak"))

    # Layers
    print("    Layers...")
    tagManagerFilePath = os.path.join(projectDirectoryPath, "ProjectSettings", "TagManager.asset")
    layers = File(tagManagerFilePath, "/0/TagManager/layers/*").readConsts()
    layers.append(Item('None', ''))
    constClasses.append(createConstClass("Layer", createConstMembers(layers)))

    # Tags
    print("    Tags...")
    tags = File(tagManagerFilePath, "/0/TagManager/tags/*").readConsts()
    tags.append(Item('None', tagManagerFilePath))
    tags.append(Item('Untagged', tagManagerFilePath))
    tags.append(Item('Respawn', tagManagerFilePath))
    tags.append(Item('Finish', tagManagerFilePath))
    tags.append(Item('EditorOnly', tagManagerFilePath))
    tags.append(Item('Player', tagManagerFilePath))
    tags.append(Item('MainCamera', tagManagerFilePath))
    constClasses.append(createConstClass("Tag", createConstMembers(tags)))

    # Scenes
    print("    Scenes...")
    scenes = Folder(os.path.join(projectDirectoryPath, "Assets"), ".unity").readConsts()
    scenes.append(Item('None', ''))
    constClasses.append(createConstClass("Scene", createConstMembers(scenes)))

    # Prefabs
    print("    Prefabs...")
    prefabs = Folder(os.path.join(projectDirectoryPath, "Assets"), ".prefab").readConsts()
    prefabs.append(Item('None', ''))
    constClasses.append(createConstClass("Prefab", createConstMembers(prefabs)))

    # GameObjects
    print("    GameObjects...")
    gameObjects = [Item('None', '')]
    for item in scenes:
        if item.path != '':
            gameObjects += File(item.path, "/*/GameObject/m_Name").readConsts()
    for item in prefabs:
        if item.path != '':
            gameObjects += File(item.path, "/*/GameObject/m_Name").readConsts()
    constClasses.append(createConstClass("GameObject", createConstMembers(gameObjects)))

    # Animator Parameters
    print("    AnimationParameters...")
    animatorParameters = [Item('None', '')]
    for item in Folder(os.path.join(projectDirectoryPath, "Assets"), ".controller").readConsts():
        if item.path != '':
            animatorParameters += File(item.path, "/*/AnimatorController/m_AnimatorParameters/*/m_Name").readConsts()
    constClasses.append(createConstClass("AnimatorParameter", createConstMembers(animatorParameters)))

    # Generate
    print("")
    print("Generating Const Classes.")
    with open(os.path.join(outputDirectoryPath, "R.cs"), "w") as file:
        print("Writing Const Classes.")
        file.write(template.render(timeStamp=datetime.datetime.utcnow(), constClasses=constClasses))
