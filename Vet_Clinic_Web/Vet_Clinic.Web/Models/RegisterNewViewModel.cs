using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        public string Password { get; set; }

        [Display(Name = "Password Confirm")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Perfil: ")]
        [UIHint("List")]
        public List<SelectListItem> Roles { get; set; }

        public string RoleName { get; set; }

        public RegisterNewViewModel()
        {
            Roles = new List<SelectListItem>();
            Roles.Add(new SelectListItem() { Value = "1", Text = "Admin" });
            Roles.Add(new SelectListItem() { Value = "2", Text = "Agent" });
            Roles.Add(new SelectListItem() { Value = "3", Text = "Doctor" });
            Roles.Add(new SelectListItem() { Value = "4", Text = "Customer" });
        }
    }
}
