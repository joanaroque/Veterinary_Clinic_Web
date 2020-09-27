using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Models
{
    public class RegisterNewViewModel : EditUserViewModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [EmailAddress]
        public string UserName { get; set; }


      


    }
}
