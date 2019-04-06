using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace OWM.Application.Services.Utils
{
    public static class Constants
    {
        public static float MarathonMiles => 26.2f;

        public static string DateFormat => "yyyy/MM/dd";
        public static string DateFormat_LongMonth => "dd MMMM yyyy";
        public static string DateTimeFormat_LongMonth => "dd MMMM yyyy at HH:mm";

        public static string GetProfilePictures(string url)
        {
            return string.IsNullOrEmpty(url)
                ? "/img/img_Plaaceholder.jpg"
                : url;
        }


        public static List<SelectListItem> GetDays()
        {
            List<int> d  = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                d.Add(i);
            }

            return d.Select(x => new SelectListItem(x.ToString(), x.ToString())).ToList();
        }

        public static List<SelectListItem> GetMonths()
        {
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            return names.Select(x => new SelectListItem(x, (names.IndexOf(x) + 1).ToString())).ToList();
        }

        public static List<SelectListItem> GetYears()
        {
            List<int> d = new List<int>();
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                d.Add(i);
            }

            return d.Select(x => new SelectListItem(x.ToString(), x.ToString())).ToList();
        }
    }
}