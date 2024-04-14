namespace ReplicaSet.Sevenet;

using System.CommandLine;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

partial class Program {

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
		Command migrateCommand = new("migrate");

		rootCommand.Add(generateDataCommand);
		rootCommand.Add(migrateCommand);


        generateDataCommand.SetHandler(GenerateData, fileName, dataCount);
        migrateCommand.SetHandler(Migrate);

        return await rootCommand.InvokeAsync(args);
    }

    private static void Migrate() {
		Console.WriteLine("Migrate");
    }

    private static void GenerateData(string fileName, int dataCount) {
		List<Data> data = [];
		for (int i = 0; i < dataCount; i++) {

			DateTime birthDate = Faker.Date.Birthday();

			data.Add(new() {
				Name = Faker.Name.FullName(),
				Age = (byte)GetAge(birthDate),
				CreatedAt = Faker.Date.Between(birthDate, DateTime.Today),
				Email = Faker.Internet.Email()
			});
		}

		string serialized = JsonSerializer.Serialize(data.ToArray());

		using FileStream? sw = File.Create(fileName);
		sw.Write( Encoding.UTF8.GetBytes(serialized) );
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