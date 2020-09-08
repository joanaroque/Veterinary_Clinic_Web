using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class NotInWeekendsValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;

            bool isWeekDay = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

            return isWeekDay;

        }
    }
}