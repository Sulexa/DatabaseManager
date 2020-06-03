# DatabaseManager

Application de gestion de base de données réalisé début 2020.

Cet outil permet de suivre l'avancement des migrations sur nos différents environments et de les mettre à jour en quelques cliques.

Il permet aussi d'appliquer différents scripts sur les bases.

## Context

Afin de facilité le passage en code first de mon entreprise j'ai créer pour répondre a un besoin de suivi des migrations et de simplification des mises en productions

## Technologie

### Back

* .NET Core 3.1
* Entity Framework Core: library utilisé pour la gestion code first de base de données
* Swagger: Permet de gérer facilement la documentation d'api

### Front
* Angular 8
* NSWAG: outil utilisant le json produit par swagger pour généré un client .net ou angular, utilisé ici pour généré le client angular.