namespace Lab_A.GEN.Models;

public class InventoryJson
{
    public string? InventoryName { get; set; }
    public double Price { get; set; }
    public override string ToString()
    {
        return $"Name: {this.InventoryName}, Price: {this.Price}";
    }
}