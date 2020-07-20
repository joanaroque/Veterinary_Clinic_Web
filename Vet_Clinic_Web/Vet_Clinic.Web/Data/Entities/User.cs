using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class User : IdentityUser
    {

        [Required(ErrorMessage = "Must insert the {0}")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


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
        public DateTime? DateOfBirth { get; set; }



        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }
                return $"https://webvetclinicjoana.azurewebsites.net{this.ImageUrl.Substring(1)}";
            }
        }

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

    }
}
