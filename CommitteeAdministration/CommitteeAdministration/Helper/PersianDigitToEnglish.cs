using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtension
    {
        private static readonly char[] PersianDegits = {'۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'};
        private static readonly char[] EnglishDegits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        /// <summary>
        /// Converts the specified persian string.
        /// </summary>
        /// <param name="persianString">The persian string.</param>
        /// <returns></returns>
        public static string ConvertPersianToEnglish(this string persianString)
        {
            var englishString = persianString;
            for (var i = 0; i < 10; i++)
            {
                englishString=englishString.Replace(PersianDegits[i], EnglishDegits[i]);
            }
            return englishString;
        }
        /// <summary>
        /// Determines whether the specified persian string is persian.
        /// </summary>
        /// <param name="persianString">The persian string.</param>
        /// <returns>
        ///   <c>true</c> if the specified persian string is persian; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPersian(this string persianString)
        {
            for (var i = 0; i < 10; i++)
            {
                if (persianString.Contains(PersianDegits[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}