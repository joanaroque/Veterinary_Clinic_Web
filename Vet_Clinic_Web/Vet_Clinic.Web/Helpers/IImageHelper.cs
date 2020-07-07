using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imagefile, string folder);
    }
}
