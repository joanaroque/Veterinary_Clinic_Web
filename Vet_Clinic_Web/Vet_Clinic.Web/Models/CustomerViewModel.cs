using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Models
{
    public class CustomerViewModel : Customer
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
