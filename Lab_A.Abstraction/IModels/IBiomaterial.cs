namespace Lab_A.Abstraction.IModels;

public interface IBiomaterial
{
    public int BiomaterialId { get; set; }
    
    public string BiomaterialName { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public ICollection<IAnalysisBiomaterial> AnalysisBiomaterials { get; set; }
    
    public ICollection<IBiomaterialCollection> BiomaterialCollections { get; set; }
}