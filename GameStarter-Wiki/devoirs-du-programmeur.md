Tout programmeur doit respecter ces règles. C'est la responsabilité de tous afin de conserver un
environnement de travail agréable.

>>>

**Sommaire**

[[_TOC_]]

>>>

## Au sujet du dépôt Git

### Envoyer du code fonctionnel sur le dépôt

> Si un développeur envoie une nouvelle version sur le dépôt Git, il est de sa responsabilité de
s'assurer que le projet compile et est fonctionnel. Dans le cas contraire, il deviendra 
le `2 de Pique` du projet jusqu'à ce qu'un nouveau `2 de Pique` soit couronné.

Prenez note que chaque nouvelle version sur la branche `develop` déclanche un `build`. Si ce `build`
échoue, vous en serez notifié par courriel.

### Ne pas envoyer de fichiers temporaires sur le dépôt

> Si un développeur envoie une nouvelle version sur le dépôt Git, il est de sa responsabilité de
s'assurer que le projet ne contient pas de fichiers temporaires.

Par mesure de propreté, n'envoyez jamais de fichiers temporaires (tel que des builds) sur le dépôt.
En temps normal, le fichier `.gitignore` (à la racine du dépôt) devrait couvrir la liste de tous les
fichiers à ignorer. Cependant, ce n'est pas infaillible : prenez tout de même le temps de consulter 
la liste des fichiers ajoutés/modifiés à chaque révision.

### Créer des messages pertinents et lisibles

> Si un développeur envoie une nouvelle version sur le dépôt Git, il est de sa responsabilité
d'y ratacher un message pertinent et complet.

Pour des questions d'historique, faites des `commit messages` détaillant ce que contient la
révision. Nul besoin d'être très exhaustif : juste mentionner les changements en général.

En général, il est beaucoup plus facile de faire un bon `commit message` si le commit est petit.
Une bonne habitude à prendre est donc de faire des petites révisions.

>>>

{+ Correct +} :thumbsup:
```
Added 'HealthPoints' to ennemies.
```

```
Removed deprecated `WorldManager` class in favor of `WorldStack`.
```

```
Fixed bug where the player would have been unable to move if they press jump while killing an ennemi.
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```
Fixed stuff.
```

```
jhfksfsdfjkfsd
```

>>>

### Créer les changements dans des *Feature Branches*

> Si un développeur désire créer une fonctionalité, il est de sa responsabilité de créer l'`Issue` correspondante
et de la réaliser dans une branche à part.

Chaque changement devra préalablement être inscrit dans une `Issue`. Lorsque vous êtes prêt à réaliser ce 
changement, créez une branche à partir de la dernière révision de la branche `develop` (vous pouvez le faire
directement sur `GitLab` à partir de l'`Issue`). Notez que le nom des branches doit suivre la nomenclature décrite 
ci-dessous.

| Type de changement | Préfix de la branche | Exemple                               |
| ------------------ | -------------------- | ------------------------------------- |
| Fonctionalité      | `feature`            | `feature/854-main-menu`               |
| Bogue non urgent   | `bug`                | `bug/421-cant-jump-off-ladder`        |
| Bogue urgent       | `hotfix`             | `hotfix/687-cant-win-tutorial-level`  |
| Corvée             | `chore`              | `chore/22-add-missing-tooltips`       |

### Fusionner un changement que si deux autres programmeurs approuvent

> Si un développeur crée une `Merge Request`, il est de sa responsabilité d'attendre que deux
autres programmeurs l'approuve avant de la fusionner.

> Lorsqu'un développeur crée une `Merge Request`, il est de la responsabilité des autres développeurs
de la réviser.

Pour approuver ou refuser une `Merge Request`, utilisez les boutons :thumbsup: et :thumbsdown:. Il 
n'est pas possible de forcer cette règle, mais il est tout de même souhaité que tous les développeurs
la respectent.

## Au sujet de Unity

### Importer les *Assets* dans Unity

> Si un développeur ajoute un `Asset` dans le projet, il est de sa responsabilité d'ouvrir Unity au
moins une fois afin de générer les fichiers `.meta` **avant** de l'envoyer sur le dépôt Git.

La raison est simple : Unity utilie des `GUID` pour faire référence à un `Asset`. Les `GUID` sont
générés dès que Unity voit un nouveau fichier, ce qui produit un nouveau fichier `.meta`. Si un 
développeur ajoute un `Asset` et l'envoie sur le dépôt sans son `.meta`, plusieurs `GUID` différents
seront générés par les autres développeurs et des conflits incorrigeable surviendront. 

### Verouiller les scènes à modifier dans Unity

> Si un développeur veut modifier une scène, il est de sa responsabilité de notifier les autres 
développeurs via `Discord`.

Effectuer des fusion de scène est pratiquement impossible. Dans bien des cas, cela peut être corrigé
en utilisant des `Prefabs`, mais il peut arriver qu'il soit tout de même nécessaire de modifier une
scene.

## Au sujet du code

### Respecter le code des autres.

> Si un développeur désire apporter des changements à un bout de code développé activement par un autre, il est de
sa responsabilité d'en discuter avec les personnes concernés au lieu de faire ces changements sans préavis.

Petite nuance à apporter à cette règle : aucun bout de code n'appartient exclusivement à une personne et aucune 
*chasse gardée* ne sera tolérée. Par contre, lors de changements importants, il est préférable d'en discuter avec 
l'auteur initial ou avec le restant de l'équipe.

### Regénérer le code régulièrement

> Il est de la responsabilité des développeur de regénérer le code régulièrement.

Le projet contient un générateur de code, plus précisément de constantes diverses. Il contient,
entre autre, la liste des `Layers`, la liste des `Tags` et la liste des `GameObjects`. Un développeur
devrait donc regénérer ces constantes dans les situations suivantes :
 * Après chaque `Pull`.
 * Avant chaque `Push`.
 * Après avoir modifié les `Layers` ou les `Tags`.
 
Le script pour générer le code se nomme `GenerateCode.bat` et se trouve à la racine du projet. Il est
aussi possible d'exécuter ce script directement à partir du Unity dans le 
menu `Tools/Project/Generate Const Classes`.
