using Microsoft.AspNetCore.Mvc;

namespace Vet_Clinic.Web.Controllers
{
    public class QrcodeController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }
    }
}