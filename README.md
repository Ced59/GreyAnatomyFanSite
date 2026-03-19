# GreyAnatomyFanSite - refonte .NET 10 / Clean Architecture

Ce dépôt correspond au **premier lot de refonte** du vieux projet Grey's Anatomy Fans.

## Objectifs déjà posés dans ce starter

- passage en **.NET 10**
- **ASP.NET Core MVC**
- **EF Core 10**
- **PostgreSQL**
- **ASP.NET Core Identity**
- **MediatR** branché pour apprentissage et usage réel
- conservation du **design legacy**
- suppression complète de la dette historique ADO.NET / Session / hash MD5

## Structure de solution

- `src/GreyAnatomyFanSite.Domain`
- `src/GreyAnatomyFanSite.Application`
- `src/GreyAnatomyFanSite.Infrastructure`
- `src/GreyAnatomyFanSite.Web`
- `tests/GreyAnatomyFanSite.Tests.Unit`
- `tests/GreyAnatomyFanSite.Tests.Integration`

## Ce qui est déjà migré

- socle Clean Architecture
- configuration MVC / Identity / EF Core / PostgreSQL
- schémas PostgreSQL `app` et `identity`
- base MediatR avec pipeline behavior de logging
- gestion des membres :
  - inscription
  - connexion
  - déconnexion
- contenu :
  - page d'accueil
  - listing des catégories
  - détail d'article
  - ajout de commentaire sur un article
- seed de démonstration :
  - rôles
  - 2 comptes utilisateurs
  - catégories
  - 1 article
  - 1 commentaire

## Comptes de démonstration

- Administrateur  
  `admin@greys.local` / `Admin123!`

- Coeur  
  `coeur@greys.local` / `Coeur123!`

## Ce qui reste volontairement en placeholder

Les modules suivants ont été laissés en écran de transition :

- série / saisons / épisodes
- personnages
- acteurs
- relations
- patients
- sondages
- administration détaillée
- photos / vidéos
- profil membre avancé
- reset password
- uploads avatars

Le but était de **démarrer la refonte proprement**, sans t'enfermer dans une migration géante d'un seul coup.

## Démarrage local

### 1. Pré-requis

- SDK **.NET 10**
- PostgreSQL
- éventuellement un IDE comme Rider / Visual Studio

### 2. Configurer la connexion PostgreSQL

Modifier `src/GreyAnatomyFanSite.Web/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=greysanatomyfans;Username=postgres;Password=postgres"
  }
}
```

### 3. Lancer l'application

```bash
dotnet restore
dotnet build
dotnet run --project src/GreyAnatomyFanSite.Web
```

## Important sur la base

L'initialisation passe désormais par `Database.MigrateAsync()` afin de rester compatible avec les migrations EF Core dès le début du projet.

Les commandes usuelles sont :

```bash
dotnet ef migrations add NomDeLaMigration --project src/GreyAnatomyFanSite.Infrastructure --startup-project src/GreyAnatomyFanSite.Web
dotnet ef database update --project src/GreyAnatomyFanSite.Infrastructure --startup-project src/GreyAnatomyFanSite.Web
```

## Pourquoi MediatR est là

Tu voulais l'ajouter dans un but éducatif. Le starter montre déjà :

- des **Queries** (`GetHomePageQuery`, `GetArticleDetailsQuery`)
- des **Commands** (`RegisterMemberCommand`, `LoginMemberCommand`, `AddArticleCommentCommand`)
- un **pipeline behavior** (`RequestLoggingBehavior<,>`)

Ça te donne une vraie base pour comprendre :
- où passent les requêtes
- où mettre la logique applicative
- comment brancher les comportements transverses

## Suite logique que je recommande

### Lot 2
- finaliser les écrans membres encore morts/incomplets du legacy
- migrer l'administration des membres
- continuer sur les modules série / personnages / acteurs
- profil
- avatar
- changement de mot de passe
- rôles / administration

### Lot 3
- migration **articles / catégories / médias / commentaires admin**

### Lot 4
- migration **série / saisons / épisodes / TMDB**

### Lot 5
- migration **personnages / acteurs / relations / patients / photos**

## Remarque importante

Je n'ai pas pu compiler ce starter dans l'environnement de génération, car le SDK .NET n'y est pas installé.  
Le code a été structuré pour être cohérent et directement exploitable chez toi, mais il faudra évidemment lancer un vrai `restore/build` local pour ajuster les éventuelles petites erreurs de surface.
