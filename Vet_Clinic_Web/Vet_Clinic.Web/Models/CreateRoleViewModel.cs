using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
