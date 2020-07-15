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


        [Display(Name = "Animal")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int AnimalId { get; set; }


        [Display(Name = "Customer")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int CustomerId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }


        public IEnumerable<SelectListItem> Doctors { get; set; }

        public IEnumerable<SelectListItem> Animals { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

    }

}
