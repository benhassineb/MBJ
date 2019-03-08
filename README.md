# Introduction

Avant d'ouvrir la solution, procéder à [l'initialisation du poste de développement](###Etapes)

## Initialisation du poste de développement

### Remarques

- L'utilisation d'IIS n'est pas recommandée sur le poste développeur car elle requiert des privilèges "administrateur". La présente solution repose sur __IIS Express__ pour le développement et le débogage de l'application Web + Web API.
- Afin d'affronter dès le développement d'éventuels problèmes de sécurité lié au *cross-scripting* ou à l'utilisation de SSL, le site Web et le site de la Web API sont configurés avec des *hostnames* correspondant à deux noms de machines distincts d'un même domaine avec le port par défaut, plutôt que _localhost:port_dynamique_. Pour éviter la modification du fichier *hosts* du poste de développeur pour résoudre ces noms en 127.0.0.1, le domaine __lvh.me__ est utilisé :
  - __web.lvh.me__ ou __web.lvh.me:44443__ pour le site Web exposant l'application aurelia
  - __api.lvh.me__ pour la Web API

### Etapes

Ces étapes ne sont à effectuer qu'une seule fois. Elle permettent l'installation d'aurelia, et la configuration complète d'IIS Express pour SSL.

1. Installer la version LTS de [node.js](https://nodejs.org/en/) pour le développement avec Aurelia.
1. Installer un client [GIT](https://git-scm.com/).
1. Installer [Visual Studio Code](https://go.microsoft.com/fwlink/?Linkid=852157) - _(optionnel)_. Vous pouvez également installer les etensions suivantes :
   - AureliaEffect.aurelia,
   - msjsdiag.debugger-for-chrome,
   - EditorConfig.EditorConfig,
   - christian-kohler.path-intellisense,
   - behzad88.Aurelia,
   - dbaeumer.vscode-eslint,
   - hookyqr.beautify,
   - davidanson.vscode-markdownlint,
   - pkief.material-icon-theme,
   - ms-vscode.vs-keybindings
1. Lancer Visual Studio, aller dans le menu Tools/Options/Projects and Solutions/Web Package Management/External Web Tools : décocher __$(VSINSTALLDIR)\Web\External__
1. Créer un projet _Visual C#/Web/ASP.NET Web Application (.NET Framework)_ puis le fermer, pour s'assurer de la création d'un fichier `applicationhost.config` dans `$home/documents/iisexpress/config`.
1. Cloner le repos git __Hermes__ dans le dossier `c:\etude\source\repos\Hermes`
1. Ouvrir un prompt *powershell* __administrateur__

    ```powershell
    cd c:\etude\source\repos\Hermes\Scripts\Developpement
    .\Init-DevEnvironment-Adm.ps1
    ```

1. Avant de démarrer la solution sous Visual Studio la première fois, il est recommandé de mettre à jour les dépendances node.js du projet Web aurelia : ouvrir un prompt *powershell*

    ```powershell
    cd c:\etude\source\repos\Hermes\Source\Front
    npm install
    ```

1. Lancer la solution __Hermes.sln__
1. Ouvrir la fenêtre __Task Runner Explorer__, et vérifier le lancement de la tâche _au-build-watch_.

## Organisation des dossiers

- __Scripts__: scripts _batch_ ou _powershell_ organisés comme suit :
  - _Developpement_ : les scripts pour le poste de développement,
  - _Build_ : les scripts utilisés pour les tâches de builds/releases TFS,
  - _Installation_ : les scripts pour l'installation de l'infrastructure et des ressources Azure,
- __Source__ : code source de l'application.
- __Documentation__ : documentations techniques de la solution.
- __Configuration__ : paramétrages de configuration organisés comme suit :
  - _Developpement_ : configuration pour le poste de développement,
  - _Deploiement_ : configuration pour les déploiements sur les différents environnements.

## Organisation des sources

- La solution est à la __racine__ du dossier __`Source`__.
- Chaque projet est  placé __directement dans son dossier homonyme__ au même niveau que la solution.
  - __`Source\`__
    - `Hermes.sln`
    - __`Hermes.Entites.Commun\`__
      - `Hermes.Entites.Commun.csproj`
- Les projets de tests sont __impérativement__ nommés du même nom que leurs projets respectifs, suffixés par __.Tests__, par exemple : `Hermes.Entites.Commun` et `Hermes.Entites.Commun.Tests`
- Les classes doivent appartenir à un namespace consistant avec le nom du projet : par exemple dans le projet Hermes.Entites.Commun :

    ```C#
    namespace Hermes.Entites.Commun
    {

        public class TraceParameters
    ```

## Création des projets .NET

- Tous les projets .NET doivent sélectionner le __framework .NET 4.7.2__.
- Les projets de type __Web API__ doivent être créés à partir du modèle _Visual C#/Web/ASP.NET Web Application (.NET Framework)_.
  Puis choisir le template Empty et cocher Web API.
- Les projets de type __Tests__ doivent être créés à partir du modèle _Visual C#/Test/Unit Test Project (.NET Framework)_.
- Les autres projets sont créés à partir de _Visual C#/Class Library (.NET Framework)_

## Modes d'exécution du projet

Il y a 3 modes d'exécution du projet:

- Un mode avec uniquement Visual Studio Code, ou les appels à la Web API depuis aurelia sont bouchonnés. Ce mode est recommandé pour le développement du HTML/CSS/Ecmascript. Dans ce cas, il faut :
  - Lancer Visual Studio Code.
  - Ouvrir le dossier __`Source\Ext.Collecte.Web`__.
  - Ouvrir une fenêtre _terminal_ et taper la commande suivante pour exécuter le site web avec aurelia :

    ```powershell
    au run --watch --env mock
    ```

  _Chaque modification HTML/CSS/Ecmascript provoque la transpilation et le bundling du projet web et le rafraîchissement du navigateur._
- Un mode mixte, ou Visual Studio Code gère le site Web, et Visual Studio 2015/2017 la Web API. Dans ce cas, il faut :
  - Lancer Visual Studio Code.
  - Ouvrir le dossier __`Source\Ext.Collecte.Web`__.
  - Ouvrir une fenêtre _terminal_ et taper la commande suivante pour exécuter le site web avec aurelia :

    ```powershell
    au run --watch --env local
    ```

  - Lancer la solution dans Visual Studio 2015/2017 et démarrer le projet Web Api.

  _Chaque modification HTML/CSS/Ecmascript provoque la transpilation et le bundling du projet web et le rafraîchissement du navigateur. Les appels à la Web API depuis aurelia ne sont pas bouchonnés._
- Un mode avec uniquement Visual Studio 2015/2017 qui . Dans ce cas, il faut :
  - Lancer la solution dans Visual Studio 2015/2017 et démarrer le projet Web __et__ le projet Web API.

  _Chaque modification HTML/CSS/Ecmascript provoque la transpilation et le bundling du projet web mais pas le rafraîchissement du navigateur._

### Règles de nommages et commentaires

  Les règles sont détaillées [ici](.\ReglesNommage.md)

### Règles de codage

  Les règles sont détaillées [ici](.\ReglesCodage.md)
