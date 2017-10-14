using System.Threading.Tasks;
using CommitteeManagement.Model;
using RestSharp;

namespace CommitteeAdministration.Services.Contract
{
    public interface IMessageTerminal
    {

        /// <summary>
        /// Sends the confirmation email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="code">The code.</param>
        Task<IRestResponse> SendConfirmationEmail(User user, string code);
        /// <summary>
        /// Sends the forgot password email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="code">The code.</param>
        Task<IRestResponse> SendForgotPasswordEmail(User user, string code);

    }

}