using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class AppointmentDetailTemp : IEntity
    {
        public int Id { get; set; }


        [Required]
        public User User { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

        [Required]
        public Animal Animal { get; set; }

        [Required]
        public Customer Customer { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return Price * (decimal)Quantity; } }
    }
}
