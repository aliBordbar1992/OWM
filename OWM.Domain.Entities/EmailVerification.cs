using System;
using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class EmailVerification : Entity
    {
        public int Id { get; set; }
        public int UserIdentityId { get; set; }
        public Guid VerificatonCode { get; set; }
        public bool Expired { get; set; }

        public static EmailVerification Create(int userIdentityId)
        {
            return new EmailVerification
            {
                Expired = false,
                UserIdentityId = userIdentityId,
                VerificatonCode = Guid.NewGuid()
            };
        }
    }
}