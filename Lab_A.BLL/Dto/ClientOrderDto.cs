namespace Lab_A.BLL.Dto;

public class ClientOrderDto
{
    public int ClientOrderId { get; set; }
    
    public int Number { get; set; }
    
    public int StatusId { get; set; }
    
    public int EmployeeId { get; set; }
    
    public int ClientId { get; set; }
    
    public DateTime BiomaterialCollectionDate { get; set; }
    
    public double Fullprice { get; set; }

    public ClientDto Client { get; set; } = null!;

    public EmployeeDto Employee { get; set; } = null!;
    
    public ICollection<OrderAnalysisDto>? OrderAnalyses { get; set; }

    public ICollection<BiomaterialCollectionDto>? BiomaterialCollections { get; set; }

    public StatusDto Status { get; set; } = null!;
}