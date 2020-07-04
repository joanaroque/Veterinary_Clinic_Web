using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            return Ok(_customerRepository.GetAll());
        }
    }
}
