# Règles de nommage et commentaires

## .NET

- Les classes, enums, méthodes, constantes (ou champs `static readonly` non privés), propriétés, namespace, respectent la casse __PascalCase__, sans accentuation.
- Les paramètres des méthodes et variables locales sont en __camelCase__, sans accentuation.
- Les champs privés sont en __camelCase__ précédés d'un `underscore`.
- Exemple :

  ``` C
  namespace Ext.Collecte
  {
      public class ModeVirement
      {
          private const int Constant = 123;
          public static readonly int ValeurFixe = 345;

          private string _champPrive;
          private static readonly string _test = "test";

          public string Nom { get; set; }

          public void DoAbbattement(string parametre)
          {
          }
      }
  }
  ```

- A part les méthodes, tous les termes sont nommés aves des noms communs, en français.
- Les méthodes sont nommées en commençant par un verbe à l'infinitif. Pour des raisons de commodités, les verbes techniques anglais `Get, Set, Read, Write, Update, Delete, Remove, Send, Receive` seront utilisés. Le reste est en français, sans accuentation :
  `GetVirement()`
- Tous les éléments publics sont commentés en français.
- Le nom du fichier correspond au nom de la classe ou de l'enum.
- Il n'y a qu'une classe par fichier.

## Javascript

- Les classes respectent la casse __PascalCase__, sans accentuation.
- Les méthodes, propriétés ou champs publics, paramètres des méthodes et variables locales sont en __camelCase__, sans accentuation.
- Les champs considérés privés sont en __camelCase__ précédés d'un `underscore`.
- Exemple :

  ``` Javascript
  export class ModeVirement
  {
    _champPrive; // supposé privé
    nom;         // supposé public

    get propriete() {
    }

    doAbbattement(parametre) {
       let test = 1;
    }
  }
  ```

- A part les méthodes, tous les termes sont nommés aves des noms communs, en français.
- Les méthodes sont nommées en commençant par un verbe à l'infinitif. Pour des raisons de commodités, les verbes techniques anglais `do, get, set, read, write, update, delete, remove, send, receive` seront utilisés. Le reste est en français, sans accuentation :
  `getVirement()`
- Tous les éléments publics sont commentés en français.
- Le nom du fichier correspond au nom de la classe, par contre le nommage du fichier doit respecter la casse __kebab-case__, exemple :
  `filtre-recherche.js` contient la classe `FiltreRecherche`.
- Il n'y a qu'une classe par fichier.
- L'indentation doit respecter les règles `lint` du projet.

## SQL
