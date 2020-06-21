namespace Meteonel.Ingestor.DomainModel
{
    public interface IReferenceData : IEntity
    {
        string Code { get; set; }
        
        string Name { get; set; }
        
        string Description { get; set; }
    }
}