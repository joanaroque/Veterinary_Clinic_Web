using System;

namespace Vet_Clinic.Common.Models
{
    public class DoctorResponse
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string LastName { get; set; }


        public string Specialty { get; set; }


        public string MedicalLicense { get; set; }


        public string TIN { get; set; }


        public string PhoneNumber { get; set; }


        public string ImageUrl { get; set; }


        public string Email { get; set; }


        public string Schedule { get; set; }


        public string ObsRoom { get; set; }


        public string Address { get; set; }


        public DateTime? DateOfBirth { get; set; }

    }
}
