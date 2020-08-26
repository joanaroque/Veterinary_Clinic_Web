using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Models
{
    public class UserRolesViewModel 
    {

        public string RoleId { get; set; }


        public string RoleName { get; set; }



        public bool IsSelected { get; set; }
    }
}
