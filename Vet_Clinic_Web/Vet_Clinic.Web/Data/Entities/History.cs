using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Vet_Clinic.Web.CustomValidation;

namespace Vet_Clinic.Web.Data.Entities
{
    public class History : IEntity
    {
        public int Id { get; set; }


        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Description { get; set; }


        public User CreatedBy { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [CustomBirthDateValidator(ErrorMessage = "Date must be more than or equal to Today's day")]
        [NotInWeekendsValidator(ErrorMessage = "The Vet Clinic is closed on weekends")]
        public DateTime CreateDate { get; set; }


        public DateTime UpdateDate { get; set; }


        public User ModifiedBy { get; set; }


        public Pet Pet { get; set; }
    }
}
