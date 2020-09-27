using System.Collections.Generic;

namespace Vet_Clinic.Web.Models
{
    public class UserRoleViewModel
    {

        public string UserId { get; set; }



        public string Name { get; set; }



        public string UserName { get; set; }



        public bool IsSelected { get; set; }



        public RoleViewModel Roles { get; set; }

    }
}
