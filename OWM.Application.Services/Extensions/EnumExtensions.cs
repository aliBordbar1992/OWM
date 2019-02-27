using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OWM.Application.Services.Extensions
{
    public class SelectList
    {
        public static IEnumerable<SelectListItem> Of<TEnum>(bool haveDefaultValue, string defaultValueText)
        {
            Type genericType = typeof(TEnum);
            var resultList = new List<SelectListItem>();
            resultList.Add(new SelectListItem(defaultValueText, $"{-1}"));
            if (genericType.IsEnum)
            {
                foreach (TEnum item in Enum.GetValues(genericType))
                {
                    string text = item.ToString().CamelCaseToSentence();
                    Enum currentEnum = Enum.Parse(typeof(TEnum), item.ToString()) as Enum;
                    int value = Convert.ToInt32(currentEnum);

                    resultList.Add(new SelectListItem(text, $"{value}"));
                }
            }

            return resultList;
        }
        public static IEnumerable<SelectListItem> Of<TEnum>()
        {
            Type genericType = typeof(TEnum);
            var resultList = new List<SelectListItem>();
            if (genericType.IsEnum)
            {
                foreach (TEnum item in Enum.GetValues(genericType))
                {
                    string text = item.ToString().CamelCaseToSentence();
                    Enum currentEnum = Enum.Parse(typeof(TEnum), item.ToString()) as Enum;
                    int value = Convert.ToInt32(currentEnum);

                    resultList.Add(new SelectListItem(text, $"{value}"));
                }
            }

            return resultList;
        }
    }
}