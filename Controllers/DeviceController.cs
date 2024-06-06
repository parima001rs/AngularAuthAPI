using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            return Ok(await _context.Devices.ToListAsync());
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> GetDevicesbyId(string custId)
        {
            var devices = await _context.Devices
                                        .Where(d => d.CustId == custId)
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
            

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevicesbyId", 
                new { 
                    custId = device.CustId
                    }, new { Message = "Success", Device = device });
        }


        // PUT api/devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(Guid id, [FromBody] DeviceUpdateDto deviceUpdateDto)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            device.StartDate = deviceUpdateDto.StartDate;
            device.EndDate = deviceUpdateDto.EndDate;
            device.IsPlanActive = deviceUpdateDto.IsPlanActive;
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
         
        [HttpPut("{deviceId}")]
        public async Task<ActionResult<List<Device>>> UpdateDeviceDate(Device device)
        {

            var dbDevice = await _context.Devices.FindAsync(device.DeviceId);
            if (dbDevice == null)
                return BadRequest("Device not found.");

            dbDevice.StartDate = device.StartDate;
            dbDevice.EndDate = device.EndDate;
            dbDevice.IsPlanActive = device.IsPlanActive;

            await _context.SaveChangesAsync();

            return Ok(await _context.Devices.ToListAsync());
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

            // Check if a device with the same DeviceId already exists
            var existingDevice = await _context.Devices
                .AnyAsync(d => d.DeviceId == device.DeviceId);
            if (existingDevice)
            {
                return BadRequest("A device with this DeviceId already exists.");
            }

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            // Return the 'CreatedAtAction' with a success message
            return CreatedAtAction("GetDevicesbyId", 
                new { 
                    custId = device.CustId
                }, new { Message = "Success", Device = device });
        }


       

        [HttpDelete("{deviceId}")]
        public async Task<ActionResult<List<Device>>> DeleteDevice(Device device)
        {
            var dbCust = await _context.Devices.FindAsync(device.DeviceId);
            if (dbCust == null)
                return BadRequest("Device not found.");

            _context.Devices.Remove(dbCust);
            await _context.SaveChangesAsync();

            return Ok(await _context.Devices.ToListAsync());
        }
        */


    }

}
