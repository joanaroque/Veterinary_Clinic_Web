namespace Vet_Clinic.Web.Data.Entities
{
    public class Admin : IEntity
    {
        public int Id { get; set; }

        public User User { get; set; }
    }
}
