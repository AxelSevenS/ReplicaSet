namespace ReplicaSet.Sevenet;

using System.CommandLine;
using System.Text;
using MongoDB.Driver;

partial class Program {

	private static readonly string ConnectionString = "mongodb://localhost:27017";
	private static readonly string TestDatabaseName = "test";
	private static readonly string UsersCollectionName = "users";

    static async Task<int> Main(string[] args) {
		EncodingProvider provider = CodePagesEncodingProvider.Instance;
		Encoding.RegisterProvider(provider);


        Option<string> fileName = new("--file-name", getDefaultValue: () => "data.json", description: "The file name of the generated data file");
        Option<int> dataCount = new("--data-count", getDefaultValue: () => 100, description: "The number of generated data");

        RootCommand rootCommand = new();

		Command generateDataCommand = new("generate") {
			fileName,
			dataCount
		};
		Command crudCommand = new("test");

		rootCommand.Add(generateDataCommand);
		rootCommand.Add(crudCommand);


        generateDataCommand.SetHandler(GenerateData, fileName, dataCount);
        crudCommand.SetHandler(TestCRUD);

        return await rootCommand.InvokeAsync(args);
    }

    private static void TestCRUD() {
		MongoClient client = new(ConnectionString);
		IMongoDatabase db = client.GetDatabase(TestDatabaseName);
		IMongoCollection<UserData> collection = db.GetCollection<UserData>(UsersCollectionName);


		UserData user = GenerateUser();
		Console.WriteLine($"Insertion d'Utilisateurs : {user}\n");
		collection.InsertOne(user);


		Console.WriteLine($"Récupération des Utilisateurs de plus de 30 ans");
		Console.WriteLine(string.Join("\n", collection.Find(Builders<UserData>.Filter.Gt(user => user.Age, 30)).ToList()) + "\n");


		Console.WriteLine($"Modification des Utilisateurs (Ajout de 5 ans d'âge)");
		UpdateResult updateRes = collection.UpdateOne(
			Builders<UserData>.Filter.Empty,
			Builders<UserData>.Update.Inc(user => user.Age, 5)
		);
		Console.WriteLine(updateRes.IsAcknowledged + "\n");


		Console.WriteLine($"Suppression de l'Utilisateur {user.Name}");
        DeleteResult deleteResult = collection.DeleteOne(Builders<UserData>.Filter.Eq(user => user.Id, user.Id));
		Console.WriteLine(deleteResult.IsAcknowledged + "\n");
    }

    private static void GenerateData(string fileName, int dataCount) {
		List<UserData> data = [];
		for (int i = 0; i < dataCount; i++) {
			data.Add(GenerateUser());
		}

		MongoClient client = new(ConnectionString);
		IMongoDatabase db = client.GetDatabase(TestDatabaseName);
		IMongoCollection<UserData> collection = db.GetCollection<UserData>(UsersCollectionName);

		Console.WriteLine($"Ajout de tous les utilisateurs à la Base Mongo");
		collection.InsertMany(data);
    }

	private static UserData GenerateUser() {
		DateTime birthDate = Faker.Date.Birthday();

		return new() {
			Name = Faker.Name.FullName(),
			Age = (byte)GetAge(birthDate),
			CreatedAt = Faker.Date.Between(birthDate, DateTime.Today),
			Email = Faker.Internet.Email()
		};
	}
	private static int GetAge(DateTime birthDate) {
		DateTime today = DateTime.Today;

		int age = today.Year - birthDate.Year;
		if (birthDate.Date > today.AddYears(-age)) {
			age--;
		}

		return age;
	}
}