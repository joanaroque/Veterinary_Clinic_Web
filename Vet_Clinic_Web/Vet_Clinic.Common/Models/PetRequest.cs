using System;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Common.Models
{
    public class PetRequest
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Breed { get; set; }

        public int OwnerId { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

    }
}
