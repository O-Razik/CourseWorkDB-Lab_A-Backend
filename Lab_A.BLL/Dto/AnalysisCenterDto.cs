#nullable disable

namespace Lab_A.BLL.Dto;

public class AnalysisCenterDto
{
    public int AnalysisCenterId { get; set; }
    
    public int CityId { get; set; }
    
    public string Address { get; set; }
    
    public virtual CityDto City { get; set; }
}