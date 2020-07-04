using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Must insert the {0}")]
        public string Treatment { get; set; }

        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Doctor")]
        public Doctor DoctorName { get; set; }

        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Animal")]
        public Animal AnimalName { get; set; }                                    //***************************************** FALTA CLASS ANIMAL

        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Schedule")]
        public DateTime AppointmentSchedule { get; set; }

        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Customer")]
        public Customer CostumerName { get; set; }                                        //***************************************** FALTA CLASS CLIENTE

        [Display(Name = "Observations")]
        public string AppointmentObs { get; set; }

        public User User { get; set; }

    }
}
