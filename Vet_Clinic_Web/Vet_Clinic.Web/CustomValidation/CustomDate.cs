using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class CustomDate : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime > DateTime.Now;
        }


    }
}
