using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Controllers.API
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
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
