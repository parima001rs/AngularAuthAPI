using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        /*
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(await _context.Customers.ToListAsync());
        }
        */

        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer custObj)
        {
            if (custObj == null)
                return BadRequest();

            await _context.Customers.AddAsync(custObj);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Customer Created!"
            });
        }
    }
}
