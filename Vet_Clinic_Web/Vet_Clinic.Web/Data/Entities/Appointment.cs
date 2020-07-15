using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Schedule")]
        public DateTime AppointmentSchedule { get; set; }


        [Display(Name = "Observations")]
        public string AppointmentObs { get; set; }

        [Required]
        public User User { get; set; }


        public IEnumerable<AppointmentDetail> Procedures { get; set; }

    }
}
