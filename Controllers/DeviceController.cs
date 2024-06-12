using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Models;
using UserAuth.Models.Dto;

namespace UserAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeviceController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            return Ok(await _context.Devices.ToListAsync());
        }

        [Authorize]
        [HttpGet("{custId}")]
        public async Task<IActionResult> GetDevicesbyId(string custId)
        {
            var devices = await _context.Devices
                                        .Where(d => d.CustId == custId && d.IsActive)
                                        .ToListAsync();
            return Ok(devices);
        }

        [HttpPost("registerDevice")]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest("Device object is null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }
            
            //new stuff
            //var customer = await _context.Customers.FindAsync(device.CustId);
            var customer = await _context.Customers.Where(c => c.CustomerId == device.CustId).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            

            var devices = await _context.Devices.Where(d => d.CustId == device.CustId).ToListAsync();
            if (devices.Count >= customer.AllowedResources)
            {
                return Ok(new { Message = $"Device limit exceeded for {customer.Name}" });
            }

            device.StartDate = DateTime.Now;
            device.EndDate = DateTime.UtcNow.AddDays(30);
            device.IsActive = true;
            device.IsPlanActive = true;


            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevicesbyId",
                new
                {
                    custId = device.CustId
                }, new { Message = "New Device Registered Successfully!", Device = device });
        }

        /*
        [HttpPost("registerDevice")]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest("Device object is null");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }
            //new stuff
            var customer = await _context.Customers.FindAsync(device.CustId);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            var devices = await _context.Devices.Where(d => d.CustId == device.CustId).ToListAsync();
            if (devices.Count >= customer.AllowedResources)
            {
                return Ok(new { Message = $"Device limit exceeded for {customer.Name}" });
            }

            device.StartDate = DateTime.Now;
            device.EndDate = DateTime.UtcNow.AddDays(30);
            device.IsActive = true;
            device.IsPlanActive = true;


            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevicesbyId", 
                new { 
                    custId = device.CustId
                    }, new { Message = "New Device Registered Successfully!", Device = device });
        }
        */

        // PUT api/devices/f7f4c885-0dbd-46fd-b13f-543f25363859
        [HttpPut("updateDevice/{deviceId}")]
        public async Task<IActionResult> UpdateDevice(Guid deviceId, [FromBody] DeviceUpdateDto deviceUpdateDto)
        {
            Device device =  _context.Devices.Where(a => a.DeviceId == deviceId).FirstOrDefault();
            //var device = await _context.Devices.FindAsync(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            device.StartDate = deviceUpdateDto.StartDate;
            device.EndDate = deviceUpdateDto.EndDate;
            device.ModifiedOn = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //changes IsActive to false 
        [HttpPut("deleteDevice/{deviceId}")]
        public async Task<IActionResult> deleteDevice(Guid deviceId)
        {
            Device device = _context.Devices.Where(a => a.DeviceId == deviceId).FirstOrDefault();
            //var device = await _context.Devices.FindAsync(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            device.IsActive = false;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /*
         //deletes permanently from db
        [HttpDelete("deleteDevice/{deviceId}")]
        public async Task<ActionResult<List<Device>>> DeleteDevice(Guid deviceId)
        {
            Device dbCust = _context.Devices.FirstOrDefault(a => a.DeviceId == deviceId);
            if (dbCust == null)
                return BadRequest("Device not found.");

            _context.Devices.Remove(dbCust);
            await _context.SaveChangesAsync();

            return Ok(await _context.Devices.ToListAsync());
        }
        */




    }

}
