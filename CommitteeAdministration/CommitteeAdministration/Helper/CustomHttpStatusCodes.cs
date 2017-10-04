using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CommitteeAdministration.Helper
{
    public class CustomHttpStatusCodes:HttpStatusCodeResult
    {
        private static HttpStatusCodeResult _invalidFirstParameter;
        private static HttpStatusCodeResult _invalidSecondParameter;
        private static HttpStatusCodeResult _invalidThirdParameter;
        private static HttpStatusCodeResult _invalidForthParameter;
        private static HttpStatusCodeResult _invalidFifthParameter;
        public static HttpStatusCodeResult InvalidFirstParameter => _invalidFirstParameter ??
                                                                    (_invalidFirstParameter = new HttpStatusCodeResult(420, "InvalidFirstParameter"));

        public static HttpStatusCodeResult InvalidSecondParameter => _invalidSecondParameter ??
                                                                     (_invalidSecondParameter = new HttpStatusCodeResult(421, "InvalidSecondParameter"));
        public static HttpStatusCodeResult InvalidThirdParameter => _invalidThirdParameter ??
                                                                     (_invalidThirdParameter = new HttpStatusCodeResult(422, "InvalidThirdParameter"));
        public static HttpStatusCodeResult InvalidForthParameter => _invalidForthParameter ??
                                                                     (_invalidForthParameter = new HttpStatusCodeResult(423, "InvalidForthParameter"));
        public static HttpStatusCodeResult InvalidFifthParameter => _invalidFifthParameter ??
                                                                    (_invalidFifthParameter = new HttpStatusCodeResult(424, "InvalidFifthParameter"));
        public CustomHttpStatusCodes(int statusCode) : base(statusCode)
        {
        }

        public CustomHttpStatusCodes(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public CustomHttpStatusCodes(HttpStatusCode statusCode, string statusDescription) : base(statusCode, statusDescription)
        {
        }

        public CustomHttpStatusCodes(int statusCode, string statusDescription) : base(statusCode, statusDescription)
        {
        }
    }
}