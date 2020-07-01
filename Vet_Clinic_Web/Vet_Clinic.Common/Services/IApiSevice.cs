using System.Threading.Tasks;
using Vet_Clinic.Common.Models;

namespace Vet_Clinic.Common.Services
{
    public interface IApiSevice
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
