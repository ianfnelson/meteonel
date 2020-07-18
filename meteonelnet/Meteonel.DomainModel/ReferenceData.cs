namespace Meteonel.DomainModel
{
    public abstract class ReferenceData : IReferenceData
    {
        public virtual int Id { get; set; }
        public virtual  string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}