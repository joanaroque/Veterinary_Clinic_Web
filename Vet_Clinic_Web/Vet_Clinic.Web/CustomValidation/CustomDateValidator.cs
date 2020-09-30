using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class CustomDateValidator : ValidationAttribute
    {
        /// <summary>
        /// validator to be used as a dataAnotation and verifies if the input value date is more recent than now
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>true if the date is more recent than now</returns>
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime > DateTime.Now;
        }


    }
}
