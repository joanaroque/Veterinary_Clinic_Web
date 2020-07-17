namespace Vet_Clinic.Common.Models
{
    class OwnerResponse
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string LastName { get; set; }


        public string TIN { get; set; }


        public string PhoneNumber { get; set; }


        public string ImageUrl { get; set; }


        public string Email { get; set; }


        //Pet ???


        public string Address { get; set; }


        public object DateOfBirth { get; set; }


        public UserResponse User { get; set; }
    }
}
