# Configuration de MongoDB en mode ReplicaSet

Pour démarrer Mongo en mode ReplicaSet en utilisant mon Projet, il suffit d'utiliser la ligne de commande
```sh
docker compose up
```
et d'attendre quelques secondes, le temps que le ReplicaSet s'initialise automatiquement (grace à un Healthcheck Docker)

# Génération de Fausses Données

Pour initier la Génération de Donnée on dispose de deux options :

- Utiliser le CLI .NET pour éxecuter le script en mode débug (requiert l'installation de .NET 8) avec la commande
	```sh
	dotnet run generate
	```
- Se servir de la version pré-compilée du script correspondant à votre OS, que vous pouvez trouver dans les releases du Repository ([Ici](https://github.com/AxelSevenS/ReplicaSet/releases/)), avec l'argument de command "generate"

Il est accéssoirement possible de préciser le nombre de données générées grace à l'argument "--data-count", sachant que la valeur par défaut est 100.

# Manipulations via la CLI MongoDB

### Insertion de Donnée

Commande
```js
db.users.insertOne({"name": "Axel Sevenet", "age": 21, "email": "axelsevenet@gmail.com", "createdAt": new Date()})
```
Résultat
```js
{
  acknowledged: true,
  insertedId: ObjectId('6620181fd354fc5a477b2da9')
}
```

### Affichage des Données

Commande
```js
db.users.find({ "age": { "$gt": 30 } })
```
Résultat
```js
{
	_id: ObjectId('66201c0e5a1020e223558492'),
	name: 'Elinor Renner',
	age: 83,
	email: 'jewel.jaskolski@okon.uk',
	createdAt: '2014-09-23T20:23:36.220Z'
}
{
	_id: ObjectId('66201c0e5a1020e223558493'),
	name: 'Josiane Rowe',
	age: 44,
	email: 'chesley@rueckersmitham.us',
	createdAt: '1996-06-18T23:19:11.213Z'
}
{
	_id: ObjectId('66201c0e5a1020e223558494'),
	name: 'Milton Feil',
	age: 48,
	email: 'cullen@murray.co.uk',
	createdAt: '1977-10-07T10:09:20.100Z'
}
{
	_id: ObjectId('66201c0e5a1020e223558497'),
	name: 'Gussie Hansen',
	age: 52,
	email: 'justine@jewess.ca',
	createdAt: '1984-06-11T08:10:12.018Z'
}
{
	_id: ObjectId('66201c0e5a1020e223558499'),
	name: 'Norberto Prohaska',
	age: 44,
	email: 'javon_boehm@kirlin.uk',
	createdAt: '1981-10-30T23:58:34.253Z'
}
{
	_id: ObjectId('66201c0e5a1020e22355849a'),
	name: 'Eda Wolf',
	age: 60,
	email: 'emelie@borergreenholt.uk',
	createdAt: '2003-04-19T00:18:56.010Z'
}
...
```

### Mise à Jour des Données

Commande
```js
db.users.updateMany({}, { "$inc": { "age": 5 } })
```
Résultat
```js
{
	acknowledged: true,
	insertedId: null,
	matchedCount: 100,
	modifiedCount: 100,
	upsertedCount: 0
}
```

### Suppression des Données

Commande
```js
db.users.deleteOne({ "name": "Axel Sevenet" })
```
Résultat
```js
{
	"acknowledged" : true,
	"deletedCount" : 1
}
```

# Automatisation avec le Langage de Programmation de votre Choix

Similairement à la génération de fausses données, il est possible d'utiliser une version précompilée du programme ou de tourner le script en mode débug pour effectuer les actions de CRUD test.

Pour Effecter les tests, il faudra donc soit :
- Utiliser le CLI .NET pour éxecuter le script en mode débug avec la commande
	```sh
	dotnet run test
	```
- Se servir de la version pré-compilée du script correspondant à votre OS, avec l'argument de command "test"