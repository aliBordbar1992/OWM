namespace OWM.Domain.Entities
{
    public class CompletedMiles : BaseAuditClass
    {
        public int Miles { get; set; }
        public virtual MilesPledged PledgedMile { get; set; }
    }
}