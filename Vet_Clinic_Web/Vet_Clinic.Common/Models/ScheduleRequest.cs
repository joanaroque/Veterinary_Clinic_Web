using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Common.Models
{
    public class ScheduleRequest
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int PetId { get; set; }


    }
}
