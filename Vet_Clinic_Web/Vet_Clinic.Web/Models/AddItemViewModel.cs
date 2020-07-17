using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int DoctorId { get; set; }


        [Display(Name = "Pet")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int PetId { get; set; }


        [Display(Name = "Owner")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int OwnerId { get; set; }


        public IEnumerable<SelectListItem> Doctors { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }

    }

}
