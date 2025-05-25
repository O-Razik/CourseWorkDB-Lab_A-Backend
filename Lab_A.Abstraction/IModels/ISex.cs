namespace Lab_A.Abstraction.IModels;

public interface ISex
{
    public int SexId { get; set; }
    
    public string SexName { get; set; }

    public ICollection<IClient> Clients { get; set; }
}