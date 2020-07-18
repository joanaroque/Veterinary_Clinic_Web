using System;

namespace Vet_Clinic.Common.Models
{
    public class AppointmentResponse
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public OwnerResponse Owner { get; set; }

        public PetResponse Pet { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime DateLocal => Date.ToLocalTime();

    }
}
