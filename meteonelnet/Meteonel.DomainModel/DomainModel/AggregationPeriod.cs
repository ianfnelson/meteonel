namespace Meteonel.DomainModel
{
    public class AggregationPeriod : IReferenceData
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual int PeriodDays { get; set; }
        public virtual int RecalculationGraceMinutes { get; set; }
    }
}
