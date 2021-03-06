﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.CustomValidation;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Pet : IEntity
    {
        [Key]
        public int Id { get; set; }


        [StringLength(30, ErrorMessage = "The {0}  that is between {2} {1} characters", MinimumLength = 1)]
        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [StringLength(30, ErrorMessage = "The {0}  that is between {2} {1} characters", MinimumLength = 3)]
        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Breed")]
        public string Breed { get; set; }


        [StringLength(10, ErrorMessage = "The {0}  that is between {2} {1} characters", MinimumLength = 3)]
        [Required(ErrorMessage = "Must insert the {0}")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Weight { get; set; }


        [Display(Name = "Photo")]
        public string ImageUrl { get; set; }


        public bool Sterilization { get; set; }


        [Required(ErrorMessage = "Must insert the {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        [CustomBirthDateValidator(ErrorMessage = "Birth Date must be less than or equal to Today's day")]
        public DateTime DateOfBirth { get; set; }


        public Specie Specie { get; set; }



        public Owner Owner { get; set; }


        public ICollection<Appointment> Appointments { get; set; }



        public User CreatedBy { get; set; }


        public DateTime CreateDate { get; set; }


        public DateTime UpdateDate { get; set; }


        public User ModifiedBy { get; set; }


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }
                return $"https://petclinicjoana.azurewebsites.net{ImageUrl.Substring(1)}";
            }
        }
    }
}
