Tout programmeur doit respecter ces règles architecturales. C'est la responsabilité de tous afin de 
conserver du code lisible et où il est plaisant de travailler.

>>>

**Sommaire**

[[_TOC_]]

>>>

## Bonnes pratiques de programmation
### Single Responsability Principle (Classes)
Une classe ne doit posséder qu'une seule et unique responsabilité ou raison d'exister. Si une classe gère plus de 
deux choses, elle doit être séparée en plusieurs classes plus petites.

### Single Responsability Principle (Méthodes)
Une méthode (ou fonction) ne doit effectuer qu'une seule action. Elle ne peut donc pas, par exemple,
modifier les points de vie du joueur et mettre à jour l'affichage en même temps. Une méthode ne peut
pas non plus consommer (utiliser) et produire (retourner) des données en même temps.

### DIY - Do It Yourself!
La programmation orientée objet est basée sur le principe d'envoi de messages (via des appels de 
méthodes) entre les objets. En ce sens, un objet doit demander à un autre d'effectuer une action au
lieu d'effectuer cette action à sa place.

Un exemple fera certainement plus de sens :

>>>

{+ Correct +} :thumbsup:
```csharp
int nbSheepsAlive = sheepfold.GetNbSheepAlive();
Debug.Log("We have " + nbSheepsAlive + " sheeps alive"); 
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
int nbSheepsAlive = 0;
foreach (Sheep sheep in sheepfold.GetSheeps()) {
    if (sheep.HealthPoints > 0) {
        nbSheepsAlive++;
    }
}
Debug.Log("We have " + nbSheepsAlive + " sheeps alive"); 
```

>>>

### DRY – Don't Repeat Yourself
Évitez qu'une partie de code se retrouve à plusieurs endroits différents. Effectuez du découpage en 
méthodes, même si ce n'est que pour une seule ligne. 

### Usage du mot clé *readonly*
Tout attribut initialisé au sein d'un constructeur et qui n'est jamais modifié doit être `readonly`.

### Règle du Brave Petit Scout
Tout scout laisse le terrain plus propre que quand il est arrivé. Cette pratique est applicable en 
programmation. Lorsque vous ouvrez un fichier, même que pour seulement le lire, et qu'un renommage, 
l'ajout d'un commentaire ou une légère correction vous semble nécessaire, faites-le. À la longue, 
ces petites modifications rendent votre code plus agréable à travailler et toute l'équipe s'en porte 
mieux.

## Registre des *Bad smells*
### Un commentaire ne sent pas bon quand…
 * Il ne vous informe en rien sur le code.
 * Il n'est pas à jour.
 * Il est une simple description du code.
 * Vous n'en comprenez pas le sens.
 * C'est du code qui est commenté.

### Une classe ne sent pas bon quand…
 * Elle dépend directement de ses classes enfants.
 * Elle n'est pas instanciée nulle part.

### Une méthode ne sent pas bon quand…
 * Il y a trop de paramètres à lui envoyer.
 * Il y a un ou des paramètres de retour.
 * Il y a un ou des paramètres *drapeaux*.
 * Elle n'est pas utilisée.
 * Elle en fait plus que ce qu'elle dit (par son nom).
 * Elle fait plus qu'une seule chose.

### Le code ne sent pas bon quand…
 * Il se répète dans un ou plusieurs fichiers à la fois.
 * Il ne sert plus.
 * Une même action est faite de plusieurs façons différentes.
 * L'algorithme utilisé est incompréhensible.
 * Il contient des *goto* ou des *instanceof*.
 * Il n'y a pas de constantes.
 * Il contient de longues conditions.