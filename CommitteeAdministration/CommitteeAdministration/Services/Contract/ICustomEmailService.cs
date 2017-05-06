using System.Threading.Tasks;
using CommitteeAdministration.Models;
using Microsoft.AspNet.Identity;
using RestSharp;

namespace CommitteeAdministration.Services.Contract
{
    public interface ICustomEmailService
    {
        Task<IRestResponse> SendEmail(RecipientUser recipientUser, string subject, string bodyMessage, string actionTitle, string actionUrl);
    }
}