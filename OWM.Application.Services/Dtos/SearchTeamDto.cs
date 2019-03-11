using System;
using OWM.Application.Services.Enums;
using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Dtos
{
    public class SearchTeamDto
    {
        public string SearchExpression { get; set; }
        public int MilesOrder { get; set; }
        public int MembersOrder { get; set; }
        public int SrchAgeRange { get; set; }
        public EnumOrderBy MilesOrderBy => (EnumOrderBy) MilesOrder;
        public EnumOrderBy MemberOrderBy => (EnumOrderBy) MembersOrder;
        public AgeRange AgeRange => (AgeRange) SrchAgeRange;
        public int Occupation { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
