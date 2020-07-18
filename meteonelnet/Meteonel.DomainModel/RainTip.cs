namespace Meteonel.DomainModel
{
    public class RainTipLatest : Reading, IRainTipReading
    {
        public virtual decimal Rain { get; set; }
    }

    public class RainTipReading : Reading, IRainTipReading
    {
        public virtual decimal Rain { get; set; }
    }

    public interface IRainTipReading : IReading
    {
        decimal Rain { get; set; }
    }
}