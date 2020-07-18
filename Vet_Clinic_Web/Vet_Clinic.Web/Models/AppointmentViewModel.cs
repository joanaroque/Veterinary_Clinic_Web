using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Models
{
    public class AppointmentViewModel : Appointment
    {

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int DoctorId { get; set; }




        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Pet")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int PetId { get; set; }




        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Owner")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int OwnerId { get; set; }



        public IEnumerable<SelectListItem> Doctors { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }


        public bool IsMine { get; set; }


        public string Reserved => "Reserved";
    }

}
