namespace OWM.Domain.Entities
{
    public class CompletedMiles : BaseAuditClass
    {
        public float Miles { get; set; }
        public virtual MilesPledged PledgedMile { get; set; }
    }
}