using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.CustomValidation
{
    public class DoctorShedule : ValidationAttribute
    {
        /// <summary>
        /// checks if the start time is greater than 9 and the end time is less than 20
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>true if the start time is greater than 9 and the end time is less than 20 </returns>
        public override bool IsValid(object value)
        {
            int workStart = 9;

            int workEnd = 20;

            int workingInput = (int)value;

            return workingInput >= workStart && workingInput < workEnd;
        }
    }
}
