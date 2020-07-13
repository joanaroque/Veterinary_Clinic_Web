using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
