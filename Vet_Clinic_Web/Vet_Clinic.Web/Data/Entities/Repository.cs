using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Data.Entities
{
    public class Repository : IRepository
    {
        //CRUD pra BD

        private readonly DataContext _context;

        public Repository(DataContext context) // injeçao
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetDoctors() //vai buscar os medicos todos
        {
            return _context.Doctors.OrderBy(p => p.Name);
        }

        public Doctor GetDoctor(int id) // vai buscar so um medico
        {
            return _context.Doctors.Find(id);
        }

        public void AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
        }

        public void RemoveDoctor(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
        }

        public async Task<bool> SaveAllAsync()
        {
            //vai á BD gravar todas as alteraçoes
            //se este nr for maior que 0 retorna true
            //ou seja se as mudanças foram 0, nao salva nada
            return await _context.SaveChangesAsync() > 0;
        }

        public bool DoctorExists(int id)
        {
            return _context.Doctors.Any(p => p.DoctorID == id);
        }
    }
}
