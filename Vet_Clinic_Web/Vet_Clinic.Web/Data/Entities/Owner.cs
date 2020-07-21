using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Owner : IEntity
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [EmailAddress]
        public string Email { get; set; }



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

        public User User { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Pet")]
        public ICollection<Pet> Pets { get; set; }



        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Appointments")]
        public ICollection<Appointment> Appointments { get; set; }


    }
}
