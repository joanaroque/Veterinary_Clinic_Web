using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class DoctorShedule : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int workStart = 9;

            int workEnd = 20;

            int workingInput = (int)value;
   
            return workingInput >= workStart && workingInput < workEnd;
        }
    }
}
