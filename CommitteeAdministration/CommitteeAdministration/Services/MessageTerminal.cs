using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Models;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using RestSharp;
using SendGrid;

namespace CommitteeAdministration.Services
{
    public class MessageTerminal : IMessageTerminal
    {
        private readonly ICustomEmailService _customEmailService = ModelContainer.Instance.Resolve<ICustomEmailService>();
        public Task<IRestResponse> SendConfirmationEmail(User user, string code)
        {
            var url = AppSetting.ClientURL + AppSetting.ConfirmEmailURL;
            var parameters = "?ucode=" + user.Id + "&code=" + code;
            var callbackUrl = url + parameters;

            var subject = "فعالسازی حساب کاربری";
            var bodyMessage = "لطفا با کلیک بر روی دکمه زیر حساب کاربری خود را فعال نمایید.";
            var actionTitle = "تایید ایمیل";
            var actionUrl = callbackUrl;
            return _customEmailService.SendEmail(WrapRecipientUser(user), subject, bodyMessage, actionTitle, actionUrl);
        }

        public void SendForgotPasswordEmail(User user, string code)
        {
            var url = AppSetting.ClientURL + "//Account/ForgotPassword";
            var parameters = "?ucode=" + user.Id + "&code=" + code;
            var callbackUrl = url + parameters;

            var subject = "بازیابی رمز عبور";
            var bodyMessage = "برای تغییر رمز عبور بر روی دکمه زیر کلیک کنید.";
            var actionTitle = "تغییر رمز عبور";
            var actionUrl = callbackUrl;
            _customEmailService.SendEmail(WrapRecipientUser(user), subject, bodyMessage, actionTitle, actionUrl);
        }

        private RecipientUser WrapRecipientUser(User user)
        {
            var recipientUser = new RecipientUser
            {
                Email = user.Email,
                FirstName = user.Name,
                LastName = user.LastName
            };

            return recipientUser;
        }

    }
}