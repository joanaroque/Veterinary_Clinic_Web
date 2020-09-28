using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class DoctorShedule : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            TimeSpan workStart = new TimeSpan(9, 0, 0);

            TimeSpan workEnd = new TimeSpan(20, 0, 0);

            TimeSpan workingHours = DateTime.Now.TimeOfDay;

            if (workStart < workEnd)
            {
                return workStart <= workingHours && workingHours <= workEnd;
            }

            return !(workEnd < workingHours && workingHours < workStart);
        }
    }
}
