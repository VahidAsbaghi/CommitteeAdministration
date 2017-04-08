using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.Services
{
    public static class ShamsiToMiladi
    {
        /// <summary>
        /// Converts the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns></returns>
        public static DateTime Convert(this string shamsiDate)
        {
            var dateTime = shamsiDate.Split('/');
            PersianCalendar persianCalendar=new PersianCalendar();
            return persianCalendar.ToDateTime(int.Parse(dateTime[0].ToEnglishNumber()),int.Parse( dateTime[1].ToEnglishNumber()),int.Parse( dateTime[2].ToEnglishNumber()), 0, 0, 0, 0, 0);
        }

        private static readonly string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        private static readonly string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string ToEnglishNumber(this string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }
        
    }

}