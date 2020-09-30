using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class NotInWeekendsValidator : ValidationAttribute
    {
        /// <summary>
        /// check if the date entered is not on weekends
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>true if the date is not on weekends</returns>
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;

            bool isWeekDay = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

            return isWeekDay;

        }
    }
}