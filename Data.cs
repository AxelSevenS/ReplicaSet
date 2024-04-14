namespace ReplicaSet.Sevenet;

using System.Text.Json.Serialization;
using System;

[Serializable]
public record class Data {
	[JsonPropertyName("name")]
	public string Name {get; set;} = "";

	[JsonPropertyName("age")]
	public byte Age {get; set;} = 0;

	[JsonPropertyName("email")]
	public string Email {get; set;} = "";

	[JsonPropertyName("createdAt")]
	public DateTime CreatedAt {get; set;} = DateTime.UnixEpoch;

}