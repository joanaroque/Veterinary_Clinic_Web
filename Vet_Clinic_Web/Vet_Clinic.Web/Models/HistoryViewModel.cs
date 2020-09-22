using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Models
{
    public class HistoryViewModel : History
    {
        public int PetId { get; set; }



        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Service Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a service type.")]
        public int ServiceTypeId { get; set; }



        public IEnumerable<SelectListItem> ServiceTypes { get; set; }
    }
}
