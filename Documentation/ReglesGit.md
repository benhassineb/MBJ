# Règles d'usage de GIT

## Règles de création d'une Pull Request

### Obligatoire

1. Appliquer les [règles de nommage](./ReglesNommage.md).
1. Vérifier que tous les fichiers du commit ont bien à être là.
1. Vérifier les modifications de chaque fichier par rapport à sa version précédente.
1. Vérifier **2 fois** toutes modifications dans les fichiers `*csproj`, `.sqlproj`, `.sln`, `.config`.
1. Avoir passé les règles de **Code Analysis** pour les projets .NET
1. Avoir passé les règles de **lint** (`au lint`) pour les fichiers javascript.
1. Avoir passé les tests .NET
1. Avoit passé les tests javascript (`au test`).

### Recommandé

1. Mettre en place les commentaires
1. Mettre en place les tests

## Règles de validation d'une Pull Request

1. Vérifier l'application des règles ci-dessus.
1. Toutes modifications subsantielles du javascript ou de l'architecture des projets (chagement de références, nouveaux projets) doivent être revues par 2 autres personnes.
