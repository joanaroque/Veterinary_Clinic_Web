using Microsoft.AspNetCore.Identity;

namespace Vet_Clinic.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
}
