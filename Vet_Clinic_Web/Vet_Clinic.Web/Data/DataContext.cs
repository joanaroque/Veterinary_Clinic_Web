using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Specie> Species { get; set; }

        public DbSet<Owner> Owners { get; set; }


        public DbSet<Assistant> Assistants { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys()
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);

            //*******************************************************************************
            modelBuilder.Entity<Specie>()
                .HasIndex(s => s.Description)
                 .IsUnique();

            modelBuilder.Entity<Doctor>()
                 .HasIndex(d => d.MedicalLicense)
                 .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.TIN)
                .IsUnique();

            modelBuilder.Entity<Assistant>()
               .HasIndex(a => a.TIN)
               .IsUnique();


            //*******************************************************************************
        }

    }
}
