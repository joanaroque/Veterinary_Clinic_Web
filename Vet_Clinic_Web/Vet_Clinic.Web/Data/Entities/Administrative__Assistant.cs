using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Administrative__Assistant : IEntity
    {

        [Key]
        public int Id { get; set; }


        public User User { get; set; }

     
    }
}
