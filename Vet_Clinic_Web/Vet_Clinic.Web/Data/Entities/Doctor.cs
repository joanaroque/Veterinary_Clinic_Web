using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Doctor : IEntity
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Specialty")]
        public string Specialty { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Currency)]
        [Display(Name = "Medical License nrº")]
        public string MedicalLicense { get; set; }

     


        [Required(ErrorMessage = "Must insert the {0}")]
        public string Schedule { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Observation Room")]
        [DataType(DataType.Currency)]
        public string ObsRoom { get; set; }


        public User User { get; set; }

      
    }
}
