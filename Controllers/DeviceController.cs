using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /*
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return Ok(await _context.Devices.ToListAsync());
        }
        */

        [HttpPost("registerDevice")]
        public async Task<IActionResult> RegisterDevice([FromBody] Device devObj)
        {
            if (devObj == null)
                return BadRequest();

            await _context.Devices.AddAsync(devObj);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Device Registered!"
            });
        }

    }
    
}
