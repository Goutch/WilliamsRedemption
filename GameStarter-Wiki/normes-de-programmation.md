Tout programmeur doit respecter ces normes. C'est la responsabilité de tous afin de conserver du code
lisible et où il est plaisant de travailler.

>>>

**Sommaire**

[[_TOC_]]

>>>

## Nommage
Tout nom doit être suffisamment clair pour comprendre instantanément ce que cela représente (ou
contient). Ne pas utiliser d'abréviations sauf si l'abréviation est plus utilisée que la forme
longue (tel que *http*). Certaines abréviations sont tout de même acceptées, telles que :

 * `min` et `max` pour *minimum* et *maximum*.
 * `nb` pour *nombre de …*.
 * `db` pour *base de données*.

Nommage toujours en anglais (les commentaires peuvent être en français).

Toujours prioriser un bon nommage à l'écriture d'un commentaire.

### Variables, attributs et paramètres - *camelCasing*
Nommés en fonction de ce que contient la variable. 

Les lettres de l'alphabet (telles que `a`, `b` ou `c`) ne peuvent être utilisées que dans des calculs
mathématiques. Ce qu'elles représentent doit être documenté avec un commentaire. `i`, `j`, et `k` sont
réservés pour les itérateurs de boucle. `e` est réservé pour les exceptions dans les blocs 
*try/catch*. Aucun préfixe de type (tel que `int`, `str` ou `flt`). Aucun préfixe de genre non plus 
(tel que `p` ou `_` pour les paramètres et `m` ou `a` pour les attributs). Évitez les répétitions : ne
suffixez pas le nom d'une variable du nom de la classe le contenant. Aussi, évitez d'inclure le type 
dans le nom lorsqu'il peut être facilement deviné.

>>>

{+ Correct +} :thumbsup:
```csharp
HighScore highScore = new HighScore();
Vector3 position = new Vector3(10,10,10);
int maxLifes = 15;
int healthPoints = 100;
```

```csharp
//Using the Law of Sines
//
//         a
//   B _________ C
//    \         /
//     \       /       a       b       c
//    c \     / b    ----- = ----- = -----
//       \   /       Sin A   Sin B   Sin C
//        \ /
//         V
//         A         where A,B,C are angles
//                   and a,b,c are lenghts
float A = 360f / 6f;
float a = 10;
float B = (180f - A) / 2f;
float b = (Sin(B) * A) / Sin(A);
```

>>>


>>>

{- Incorrect -} :thumbsdown:
```csharp
HighScore hs = new HighScore();
Vector2 positionVector2 = new Vector2(10,10);
bool boolshit = true;
int intMaxLifes = 15;
int mPlayerHeatlhPoints = 100;
```

```csharp
float v = d / t;
```

>>>

### Tableaux, listes et collections (Variables, attributs et paramètres)  - *camelCasing*
Nommés en fonction de ce que représente la collection d'éléments.

Ne pas suffixer de `list` ou de `array` : utiliser plutôt une forme au pluriel. Toujours utiliser les 
types abstraits (`IList` au lieu de `List`) pour faciliter la maintenance.

>>>

{+ Correct +} :thumbsup:
```csharp
Student[] allStudents;
IList<Employee> partTimeEmployees;
```

```csharp
IList<int> scores = new List<int>();
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
Student[] studentArray;
IList<Employee> employeeList;
```

```csharp
List<int> scores = new List<int>();
```

>>>

### Constantes et attributs statiques en lecture seule  - *PascalCasing*
Nommés en fonction de ce que représente la valeur de la constante. 

Attention à ne pas mélanger le standard Java (UPPER_CASE) avec le standard C# (PascalCase). Ne pas 
utiliser `static readonly` pour les types de base.

>>>

{+ Correct +} :thumbsup:
```csharp
private const float MarginOfError = 0.001f;
private static readonly Guid Tag = new Guid("{612E-8U16- 3E}");
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
private const float ERROR_MARGIN = 0.001f;
```

>>>

### Événements (Attributs événementiels publics) - *PascalCasing*
Nommés en utilisant une courte description de l'événement.

Toujours préfixé de `On`.

>>>

{+ Correct +} :thumbsup:
```csharp
public event KeyEventHandler OnKeyPressed;
public event DeathEventHandler OnDeath;
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public event KeyEventHandler onKeyPressed;
public event DeathEventHandler DeathEvent;
```

>>>

### Classes, structures et énumérations - *PascalCasing*
Pour une classe, nommée en fonction de sa responsabilité ou de ce qu'elle représente. Contient 
généralement un verbe d'action. Pour une structure, nommée en fonction des données qu'elle 
représente. Pour une énumération, nommée en fonction de l'ensemble de valeurs qu'elle représente.

Ne jamais utiliser le terme `Manager`, car imprécis et souvent symptomatique d'un découpage déficient.
Aussi, n'utiliser aucun préfixe, même pour les classes abstraites.

>>>

{+ Correct +} :thumbsup:
```csharp
public class ColisionSensor { }
public class HealthPoints { }
public abstract class BulletShooter { }
```

```csharp
public struct Point2D { }
```

```csharp
public enum Team { Red, Blue }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public class ColisionManager { }
public abstract class BaseBulletShooter { }
```

>>>

### Interfaces - *PascalCasing*
Nommés en fonction de la capacité introduite par l'interface à toute classe l'implémentant.

Préfixés par un `I` et se terminent souvent par `able`, dont la signification réside dans l'anglais 
*being able to …*.

>>>

{+ Correct +} :thumbsup:
```csharp
public interface IGameEntity { }
public interface IDamageable { }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public interface GameEntity { }
public interface IDamageTaker { }
```

>>>

### Méthodes et fonctions - *PascalCasing*
Nommés en lien avec l'effet produit par un appel à cette méthode. 

Contient systématiquement un verbe d'action. Éviter l'usage de plusieurs verbes consécutifs afin de 
privilégier une forme courte. Les méthodes sont des messages envoyés aux objets. Leur nom doit 
refléter ce principe en utilisant des verbes à la forme impérative. 

Certaines méthodes existent pour avertir l'objet qu'un événement s'est produit. Exceptionnellement, 
ces méthodes débutent donc par le préfixe `On` suivi d'une courte description de l'événement.

Dans le cas d'un accesseur, privilégier les propriétés sauf si la donnée retournée doit être calculée 
chaque fois qu'elle est obtenue. Dans un tel cas, utiliser les préfixes `Get`, `Set` et `Is`.

>>>

{+ Correct +} :thumbsup:
```csharp
public void Die() {}
public void OnNameChanged() { }
public string GetElapsedTimeSinceDeath (){ }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public void MakeDie() {}
public void NameHasChanged() { }
public long GetName(){ }
```

>>>

### Propriétés - *PascalCasing*
Nommés en considération de l'information que contient la propriété. 

Privilégier un accesseur (méthode) à une propriété si la donnée retournée doit être calculée chaque 
fois qu'elle est obtenue. Évitez les répétitions : ne préfixez pas le nom d'une propriété du nom de 
la classe la contenant.

>>>

{+ Correct +} :thumbsup:
```csharp
public string Name { get; set; }
public int HealthPoints { get; private set; }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public string PlayerName { get; set; }
public int ElapsedTimeSinceDeath { get; }
```

>>>

### Variables et accesseurs booléens
Lorsque sémantiquement approprié, préfixer les variables, méthodes et propriétés booléennes de `Is`.
Aussi, préférez l'usage de deux méthodes opposées pour modifier un booléen au lieu d'utiliser une 
seule méthode, comme dans l'exemple ci-dessous.

>>>

{+ Correct +} :thumbsup:
```csharp
private bool isEnabled = false;
public bool IsEnabled() { }
public void Enable() { }
public void Disable() { }
```

>>>

>>>

Critiqué :hand\_splayed: 
```csharp
public bool SetEnabled(bool isEnabled) { }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
private bool enabled = false;
public bool GetEnabled() { }
public bool SetIsEnabled(bool isEnabled) { }
```

>>>

### Delegates (et Delegates événementiels) - *PascalCasing*
Nommés en considération des retombées attendues par l'appel de la fonction déléguée.

Dans le cas d'un Delegate dédié à un événement, suffixer de `EventHandler`, mais ne pas préfixer de 
`On`.

>>>

{+ Correct +} :thumbsup:
```csharp
public delegate bool ListFilter(int number);
public delegate void DeathEventHandler();
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
public delegate bool FilterFunc(int number);
public delegate void OnDeath();
```

>>>


### Espaces de nommage - *Dot.Casing*
Nommés en fonction de la fonctionnalité gérée par les éléments de l'espace de nommage. 

Peut contenir un nom de produit. Ne contient jamais de verbe et n'est jamais au pluriel.

>>>

{+ Correct +} :thumbsup:
```csharp
namespace Tp1.Game.Ai.State { }
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
namespace Tp1.AiGameStates { }
```

>>>

## Visibilités

Les mots clés de visibilité tels que `public`, `private`, `protected` et `internal` doivent toujours
être présents, même si le langage les considère facultatifs.

Toujours utiliser la visibilité la plus restrictive possible.

**Enfin, il est strictement interdit d'avoir un attribut public.**

## Mise en forme
### Indentation, accolades « { » et « } » et longueur des lignes
Utilisez 4 espaces et non pas une tabulation pour indenter. Indentez d'un niveau à l'intérieur 
d'accolades `{` et `}`. Les accolades `{` et `}` sont toujours sur une nouvelle ligne. Vous pouvez 
ne pas utiliser d'accolades après une condition ou une boucle si l'instruction est très courte. Sinon,
toujours utiliser des accolades, même si le langage considère cela facultatif.

Une ligne de code ne doit jamais dépasser 160 caractères. Aussi, si possible, remplacez les condtions
produisant des booléens par des affectations.

>>>

{+ Correct +} :thumbsup:
```csharp
if (healthPoints > 100)
{
    EnnemyFactory.create().isBoss(true).addBehaviour(new SplitBehaviour(40));
}
```

```csharp
if (healthPoints <= 0) isDead = true;
```

```csharp
if (OnDeath != null) OnDeath();
```

```csharp
isDead = healthPoints <= 0;
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
if (healthPoints <= 0) {
    isDead = true;
}
```

```csharp
if (healthPoints <= 0) 
{
isDead = true;
}
```

```csharp
if (healthPoints > 100) EnnemyFactory.create().isBoss(true).addBehaviour(new SplitBehaviour(40));
```

```csharp
if (healthPoints <= 0)
isDead = true;
```

>>>

### Espacement vertical et regroupements (Retours de chariot)
Regroupez :
 * les variables/constantes/attributs fortement liées.
 * les lignes de code fortement liées.

Séparez chaque regroupement d'une ligne vide.

>>>

{+ Correct +} :thumbsup:
```csharp
private Texture visual;
private float minSize;
private float maxSize;

private float minExplosionForce;
private float maxExplosionForce;
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
private Texture visual;
private float maxSize;
private float minExplosionForce;
private float minSize;
private float maxExplosionForce;
```

>>>

### Nombre de classes, structure ou énumération par fichier
Une seule classe, structure ou énumération par fichier de code source. Le nom du fichier est égal au 
nom de la classe/structure/énumération qu'il contient. 

Exceptionnellement, toute énumération ou delegate possédant un lien très fort avec une classe peut 
se retrouver dans le même fichier.

### Ordre des éléments dans un fichier de code source
Les éléments à l'intérieur d'un fichier de code source doivent être placés dans cet ordre :

 1. Usings
 2. Espace de nommage
 3. Delegates
 4. Énumération / Structure / Classe

### Ordre des éléments dans une classes ou une structure
Les éléments à l'intérieur d'une classe ou d'une structure doivent être placés dans cet ordre :
 1. Constantes - *Public, suivi de privé*
 2. Attributs statiques - *Privé uniquement*
 3. Propriétés statiques - *Public, suivi de privé*
 4. Méthodes statiques - *Public et privé mélangé*
 7. Attributs - *Privé uniquement*
 6. Événements - *Public et protégé mélangé*
 7. Constructeur par défaut - *Public, privé ou protégé*
 8. Constructeurs avec paramètres - *Public, privé et protégé mélangé*
 9. Surcharges d'opérateurs - *Public, privé et protégé mélangé*
 10. Propriétés - *Public, privé et protégé mélangé*
 11. Méthodes - *Public, privé et protégé mélangé*
 12. Énumérations internes - *Public, suivi de privé et de protégé*
 13. Structures internes  - *Public, suivi de privé et de protégé*
 14. Classes internes  - *Public, suivi de privé et de protégé*

## Usage de commentaires et documentation
Les commentaires sont destinés à compenser le manque d'expressivité du langage, et non pas à 
reformuler le code en langage naturel. De tels commentaires sont comparables à de la duplication de 
code, ce qu'il faut pourtant éviter.

D'autres commentaires à éviter :
 * Les lignes de code en commentaire.
 * Les en-têtes de section de code (régions).

La maintenance des commentaires (et surtout de la documentation) est souvent laissée pour compte. Les
commentaires deviennent alors désuets, voir trompeurs. Mieux vaut laisser le code parler de lui-même
en utilisant un bon nommage et une bonne structure.

Avant d'écrire un commentaire, vérifiez s'il ne serait pas suffisant de :
 * Utiliser un meilleur nommage.
 * Redécouper le code en méthodes plus petites.
 * Redécouper la classe en classes plus petites.
 
Parmi les commentaires réellement utiles :
 * Explications sur un algorithme complexe.
 * Explications sur un calcul mathématique complexe.
 * Explications sur un Regex.
 * Documentation d'une librairie publique.
 * Commentaires légaux en haut des fichiers.

>>>

{+ Correct +} :thumbsup:
```csharp
//Matches integers (like 1 or -12)
Regex integerRegex = new Regex("^-?\\d+$");
```

```csharp
/*
 * Copyright (c) Benjamin Lemelin
 * All rights reserved.
 */
```

>>>

>>>

{- Incorrect -} :thumbsdown:
```csharp
//Health points
private int healthPoints;
```

```csharp
/// <summary>
/// Default constructor.
/// </summary>
public HitSensor() { }
```

```csharp
/*************************************
* Get circle area
************************************/
int someLogicHere1 = 1;
/*************************************
* Add damage to enemies
************************************/
int someLogicHere2 = 2;
```

```csharp
private void OnEntityHit(int hitPoints)
{
//health.Hit(hitPoints);
}
```

>>>