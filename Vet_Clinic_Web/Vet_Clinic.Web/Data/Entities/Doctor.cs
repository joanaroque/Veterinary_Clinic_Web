using System;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.CustomValidation;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Doctor : IEntity
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"\d{9}",
         ErrorMessage = "Must insert the {0} correct.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Specialty")]
        public string Specialty { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Currency)]
        [Display(Name = "Medical License nrº")]
        public string MedicalLicense { get; set; }




        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Observation Room")]
        [DataType(DataType.Currency)]
        public string ObsRoom { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Work Start")]
        public int WorkStart { get; set; }




        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Work End")]
        public int WorkEnd { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Currency)]
        public string TIN { get; set; }



        [Display(Name = "Photo")]
        public string ImageUrl { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        public string Address { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        [CustomBirthDateValidator(ErrorMessage = "Birth Date must be less than or equal to Today's day")]
        public DateTime? DateOfBirth { get; set; }



        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }
                return $"https://petclinicjoana.azurewebsites.net{ImageUrl.Substring(1)}";
            }
        }


        public User CreatedBy { get; set; }


        public DateTime CreateDate { get; set; }


        public DateTime UpdateDate { get; set; }


        public User ModifiedBy { get; set; }


    }
}
