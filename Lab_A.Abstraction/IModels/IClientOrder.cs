using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_A.Abstraction.IModels;

public interface IClientOrder
{
    public int ClientOrderId { get; set; }
    
    public int? Number { get; set; }
    
    public int StatusId { get; set; }
    
    public int? EmployeeId { get; set; }
    
    public int? ClientId { get; set; }
    
    public DateTime? BiomaterialCollectionDate { get; set; }
    
    public double? Fullprice { get; set; }
    
    public ICollection<IBiomaterialCollection> BiomaterialCollections { get; set; }
    
    public IClient Client { get; set; }
    
    public IEmployee Employee { get; set; }
    
    public ICollection<IOrderAnalysis> OrderAnalyses { get; set; }
    
    public IStatus Status { get; set; }
}