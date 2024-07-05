using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Models;
using UserAuth.Models.Dto;

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _context.Customers.Where(c => c.IsActive).ToListAsync());
        }


        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer custObj)
        {
            if (custObj == null)
                return BadRequest();

            int lastExistingCustomerId = await _context.Customers.DefaultIfEmpty().MaxAsync(c => c.Id);
            int newId = lastExistingCustomerId + 1;

            string initials = new string(custObj.Name.Take(4).ToArray());
            custObj.CustomerId = "CUST" + newId.ToString() + initials;

            custObj.IsActive = true;
            custObj.CreatedBy = custObj.CreatedBy;

            await _context.Customers.AddAsync(custObj); 
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Customer Created!"
            });
        }

        [HttpPut("updateCustomer/{customerId}")]
        public async Task<IActionResult> UpdateCustomer(string customerId, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            Customer customer = _context.Customers.Where(a => a.CustomerId == customerId).FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = customerUpdateDto.Name;
            customer.Email = customerUpdateDto.Email;
            customer.AllowedResources = customerUpdateDto.AllowedResources;
            customer.ModifiedOn = DateTime.UtcNow;
            customer.ModifiedBy = customerUpdateDto.ModifiedBy;

            try
            {
                await _context.SaveChangesAsync();
                //return Ok(customer);
                return Ok(new
                {
                    Message = "Customer is successfully updated!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPut("deleteCustomer/{customerId}")]
        public async Task<IActionResult> deleteCustomer(string customerId, CustomerDeleteDto customerDeleteDto)
        {
            Customer customer = _context.Customers.Where(a => a.CustomerId == customerId).FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            customer.IsActive = false;
            customer.ModifiedBy = customerDeleteDto.Modifiedby;
            try
            {
                await _context.SaveChangesAsync();
                //return Ok(customer);
                return Ok(new
                {
                    Message = "Customer is successfully deleted!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
