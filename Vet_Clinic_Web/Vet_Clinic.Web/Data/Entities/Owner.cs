using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Owner : IEntity
    {
        [Key]
        public int Id { get; set; }


        public User User { get; set; }


        public User CreatedBy { get; set; }


        public DateTime CreateDate { get; set; }


        public DateTime UpdateDate { get; set; }


        public User ModifiedBy { get; set; }


        public ICollection<Pet> Pets { get; set; }


        public ICollection<Appointment> Appointments { get; set; }


    }
}
