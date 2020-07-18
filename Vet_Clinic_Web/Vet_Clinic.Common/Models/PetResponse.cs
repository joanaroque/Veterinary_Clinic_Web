using System;
using System.Collections.Generic;

namespace Vet_Clinic.Common.Models
{
   public class PetResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Breed { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<HistoryResponse> Histories { get; set; }

    }
}
