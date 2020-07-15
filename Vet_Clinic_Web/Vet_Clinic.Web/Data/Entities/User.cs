using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

    }
}
