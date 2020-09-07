using System;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.CustomValidation;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Observations")]
        public string AppointmentObs { get; set; }


       
        public Doctor Doctor { get; set; }

       
        public Pet Pet { get; set; }

       
        public Owner Owner { get; set; }


        public User CreatedBy { get; set; }


        [Display(Name = "Date")]
        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [CustomDate(ErrorMessage = "Date must be more than or equal to Today's day")]
        public DateTime CreateDate { get; set; }


        public DateTime UpdateDate { get; set; }


        public User ModifiedBy { get; set; }
    }
}
