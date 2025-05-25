namespace Lab_A.Abstraction.IModels;

public interface ICity
{
    public int CityId { get; set; }

    public string CityName { get; set; }

    public ICollection<IAnalysisCenter> AnalysisCenters { get; set; }

    public ICollection<ILaboratory> Laboratories { get; set; }
}