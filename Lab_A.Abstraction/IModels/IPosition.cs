namespace Lab_A.Abstraction.IModels;

public interface IPosition
{
    public int PositionId { get; set; }

    public string PositionName { get; set; }

    public ICollection<IEmployee> Employees { get; set; }
}