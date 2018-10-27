# Jeu - Projet de départ

```
À faire : Écrire une courte description du projet ici.
```

```
À faire : Mettre à jour les liens dans ce document vers votre dépôt. 
Par exemple : 
   https://gitlab.com/csf-game-dev/projet-synthese/game-starter.git
```

## Démarrage rapide

Ces instructions vous permettront d'obtenir une copie opérationnelle du projet sur votre machine à des fins de développement.

### Prérequis

* [Git](https://git-scm.com/downloads) - Système de contrôle de version. Utilisez la dernière version.
* [Python 3.6.5](https://www.python.org/downloads/) - Pour exécuter divers scripts. Toute version au dessus de 3.6.5 *devrait* faire l'affaire.
* [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio] (https://www.visualstudio.com/fr/) - IDE. Vous pouvez utiliser également n'importe quel autre IDE: assurez-vous simplement qu'il supporte les projets Unity.
* [Unity 2018.1.6f1](https://unity3d.com/fr/get-unity/download/) - Moteur de jeu. Veuillez utiliser **spécifiquement cette version **. Attention à ne pas installer Visual Studio si vous avez déjà un IDE. Vous pouvez aussi utiliser Unity Hub pour effectuer l'installation.

*Attention: actuellement, seul le développement sur Windows est supporté.*

### Compiler une version de développement

Tout d'abord, vérifiez que `git` et` python` sont présents dans votre variable d'environnement `PATH`.

```
git --version
python --version
```

Ensuite, clonez le projet **en vous assurant qu'il n'y a pas d'espace dans le chemin vers le dossier de destination.**

```
cd /folder/with_no_space/
git clone https://gitlab.com/csf-game-dev/projet-synthese/game-starter.git Game-Starter
```

Avant d'ouvrir le projet dans Unity, exécutez le script `GenerateCode.bat`. Ce dernier va générer du code C # (principalement des constantes). Cela pourrait prendre un certain temps.

```
cd Shipwrecked
./GenerateCode.bat
```

Notez que vous aurez à régénérer le code régulièrement.

Enfin, ouvrez le projet dans Unity. Laissez-le importer tous les `assets` du projet. Ensuite, allez dans `File > Build Settings…` et compilez une version Windows X64.

## Tester un version stable ou de développement

Téléchargez l'un des fichiers zip suivants. Décompressez-le dans n'importe quel dossier et lancez le fichier `.exe`.

* [Master (Stable)](https://gitlab.com/csf-game-dev/projet-synthese/-/jobs/artifacts/master/download?job=build)
* [Develop (Dernière version)](https://gitlab.com/csf-game-dev/projet-synthese/-/jobs/artifacts/develop/download?job=build)

La construction `develop` est la dernière version du projet. Elle est assez instable, mais aura les fonctionnalités les plus récentes.
La construction `master` est la dernière version stable du projet.

Si vous rencontrez un bogue, vous êtes priés de le [signaler](https://gitlab.com/csf-game-dev/projet-synthese/issues/new?issuable_template=Bug). Veuillez fournir une explication détaillée de
votre problème avec les étapes pour reproduire le bogue. Les captures d'écran et les vidéos jointes sont les bienvenues.

## Contribuer au projet

Veuillez lire [CONTRIBUTING.md](CONTRIBUTING.md) pour plus de détails sur notre code de conduite.

Aussi, veuillez lire notre [Wiki](https://gitlab.com/csf-game-dev/projet-synthese/wikis/home) sur notre [processus de création](https://gitlab.com/csf-game-dev/projet-synthese/wikis/agile),
nos [normes](https://gitlab.com/csf-game-dev/projet-synthese/wikis/normes-de-programmation) et
vos [devoirs](https://gitlab.com/csf-game-dev/projet-synthese/wikis/devoirs-du-programmeur) en tant que contributeur.

## Plateformes utilisés pour le développement

* [Unity](https://unity3d.com) - Moteur de jeu.
* [Gitlab CI](https://about.gitlab.com/features/gitlab-ci-cd/) - Système d'intégration continue

## Auteurs

```
À faire : Ajoutez vous noms ici ainsi que le nom de tout artiste ayant participé au projet 
(avec lien vers leur portfolio s'il existe).

Inscrivez aussi, en détail, ce sur quoi chaque membre de l'équipe a principalement travaillé.
```

* **Benjamin Lemelin** - *Programmeur*
  * Extensions sur le moteur Unity pour la recherche d'objets et de composants. Générateur de constantes. Gestionnaire de
    chargement des scènes.
* **Mathieu Bédard** - *Programmeur*
* **Simon Robidas** - *Programmeur*
* **Gabriel Bouchard** - *Programmeur*
* **Jonathan Rhéaume** - *Programmeur*
* **Prénom Nom** - *Concepteur sonore*
* **Prénom Nom** - *Artiste 2D et Artiste UI*

## Remierciements

```
À faire : Remercier toute personne ayant contribué au projet, mais qui n'est pas un auteur.
```
