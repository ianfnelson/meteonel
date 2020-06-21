namespace Meteonel.Ingestor.DomainModel
{
    public class Device : IEntity
    {
        public virtual int Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string Location { get; set; }
    }
}