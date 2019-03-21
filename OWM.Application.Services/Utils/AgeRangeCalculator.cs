using System;
using System.ComponentModel;
using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Utils
{
    public class AgeRangeCalculator
    {
        public static AgeRange GetAgeRange(DateTime dateOfBirth)
        {
            int years = GetYears(dateOfBirth);
            if (years <= 14) return AgeRange._Below14;
            if (years >= 15 && years <= 17) return AgeRange._15To17;
            if (years > 17) return AgeRange._17Plus;

            throw new InvalidEnumArgumentException("no age group found");
        }

        private static int GetYears(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Now;
            int years = new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1;

            return years;
        }

        public static string GetAgeRangeCaption(AgeRange ageRange)
        {
            switch (ageRange)
            {
                case AgeRange._15To17:
                    return "between 15 to 17 years old";
                case AgeRange._17Plus:
                    return "above 17 years old";
                case AgeRange._Below14:
                    return "below 14 years old";
            }

            throw new ArgumentException("Age group is undefined.");
        }
    }
}