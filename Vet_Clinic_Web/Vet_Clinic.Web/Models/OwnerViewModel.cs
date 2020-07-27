﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Models
{
    public class OwnerViewModel : Owner
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}