namespace Lab_A.BLL.Dto;

public class OrderAnalysisDto
{
    public int OrderAnalysisId { get; set; }

    public int AnalysisId { get; set; }

    public int ClientOrderId { get; set; }
    
    public virtual AnalysisDto Analysis { get; set; } = null!;

    public virtual ClientOrderDto? ClientOrder { get; set; }
}