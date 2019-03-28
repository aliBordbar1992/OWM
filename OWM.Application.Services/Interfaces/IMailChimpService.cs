using System.Threading.Tasks;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Interfaces
{
    public interface IMailChimpService
    {
        Task AddMemberToList(MailChimpMemberDto member);
    }
}
