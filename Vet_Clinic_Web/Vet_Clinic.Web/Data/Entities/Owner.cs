using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Owner : IEntity
    {
        [Key]
        public int Id { get; set; }


        public User User { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Pet")]
        public ICollection<Pet> Pets { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Appointments")]
        public ICollection<Appointment> Appointments { get; set; }


    }
}
