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

            throw new InvalidEnumArgumentException("no age range found");
        }

        private static int GetYears(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Now;
            int years = new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1;

            return years;
        }
    }
}