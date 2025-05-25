using Lab_A.GEN.Helpers;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab_A.GEN.Models;

public class AnalysisJson
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }

    [JsonConverter(typeof(BiomaterialsConverter))]
    public BiomaterialJSON[]? Biomaterials { get; set; }
    public int Price { get; set; }

    public override string ToString()
    {
        return $"Name: {this.Name}, Category: {this.Category}, Price: {this.Price}";
    }
}