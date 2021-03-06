﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Models
{
    public class PetViewModel : Pet
    {
        public int OwnerId { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Pet Specie")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Specie.")]
        public int SpecieId { get; set; }


        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        public IEnumerable<SelectListItem> Species { get; set; }

        public string OwnerFullName { get; set; }

        public int HistoriesCount { get; set; }
    }
}
