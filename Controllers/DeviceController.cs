﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        /*
        [HttpGet("getbyDevId/{deviceId}")]
        public async Task<IActionResult> GetDevicesbyDeviceId(String deviceId)
        {
            var devices = await _context.Devices
                                        .Where(d => d.DeviceId == deviceId && d.IsActive)
                                        .ToListAsync();
            return Ok(devices);
        }
        */

        [HttpGet("getbyDevId/{deviceId}")]
        public async Task<IActionResult> GetDevicesbyDeviceId(string deviceId)
        {
            var devices = await _context.Devices
                                        .Where(d => d.DeviceId == deviceId && d.IsActive)
                                        .Select(d => new
                                        {
                                            d.DeviceId,
                                            d.CustId,
                                            d.StartDate,
                                            d.EndDate,
                                            d.IsActive,
                                            d.IsPlanActive,
                                            Customer = new
                                            {
                                                d.Customer.ClientId,
                                                d.Customer.ClientSecret
                                            }
                                        })
                                        .ToListAsync();

            return Ok(devices);
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

            device.StartDate = DateTime.UtcNow;
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
        */
        /*
        [HttpPost("registerDevice")]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest(new { Message = "Device object is null", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid model object", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null });
            }

            // Check if the device ID already exists
            var existingDevice = await _context.Devices.Where(d => d.DeviceId == device.DeviceId).FirstOrDefaultAsync();
            if (existingDevice != null)
            {
                return BadRequest(new { Message = "Device ID already exists", startDate = existingDevice.StartDate, endDate = existingDevice.EndDate, isPlanActive = existingDevice.IsPlanActive, isActive = existingDevice.IsActive });
            }

            var customer = await _context.Customers.Where(c => c.CustomerId == device.CustId).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null });
            }

            var devices = await _context.Devices.Where(d => d.CustId == device.CustId).ToListAsync();
            var activeDevices = await _context.Devices.Where(d => d.CustId == device.CustId && d.IsActive).ToListAsync();
            if (activeDevices.Count >= customer.AllowedResources)
            {
                return BadRequest(new { Message = $"Device limit exceeded for {customer.Name}", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null });
            }

            device.StartDate = DateTime.UtcNow;
            device.EndDate = DateTime.UtcNow.AddDays(30);
            device.IsActive = true;
            device.IsPlanActive = true;

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevicesbyId",
                new
                {
                    custId = device.CustId
                }, new { Message = "New Device Registered Successfully!", startDate = device.StartDate, endDate = device.EndDate, isPlanActive = device.IsPlanActive, isActive = device.IsActive});
        }
        */

        [HttpPost("registerDevice")]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest(new { Message = "Device object is null", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null, clientId = (string)null, clientSecret = (string)null });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid model object", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null, clientId = (string)null, clientSecret = (string)null });
            }

            // Check if the device ID already exists
            var existingDevice = await _context.Devices.Where(d => d.DeviceId == device.DeviceId).FirstOrDefaultAsync();
            if (existingDevice != null)
            {
                return BadRequest(new { Message = "Device ID already exists", startDate = existingDevice.StartDate, endDate = existingDevice.EndDate, isPlanActive = existingDevice.IsPlanActive, isActive = existingDevice.IsActive, clientId = (string)null, clientSecret = (string)null });
            }

            var customer = await _context.Customers.Where(c => c.CustomerId == device.CustId).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null, clientId = (string)null, clientSecret = (string)null });
            }

            var devices = await _context.Devices.Where(d => d.CustId == device.CustId).ToListAsync();
            var activeDevices = await _context.Devices.Where(d => d.CustId == device.CustId && d.IsActive).ToListAsync();
            if (activeDevices.Count >= customer.AllowedResources)
            {
                return BadRequest(new { Message = $"Device limit exceeded for {customer.Name}", startDate = (DateTime?)null, endDate = (DateTime?)null, isPlanActive = (bool?)null, isActive = (bool?)null, clientId = (string)null, clientSecret = (string)null });
            }

            device.StartDate = DateTime.UtcNow;
            device.EndDate = DateTime.UtcNow.AddDays(30);
            device.IsActive = true;
            device.IsPlanActive = true;

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevicesbyId",
                new
                {
                    custId = device.CustId
                }, new { Message = "New Device Registered Successfully!", startDate = device.StartDate, endDate = device.EndDate, isPlanActive = device.IsPlanActive, isActive = device.IsActive, clientId = customer.ClientId, clientSecret = customer.ClientSecret });
        }



        // PUT api/devices/f7f4c885-0dbd-46fd-b13f-543f25363859
        [HttpPut("updateDevice/{deviceId}")]
        public async Task<IActionResult> UpdateDevice(String deviceId, [FromBody] DeviceUpdateDto deviceUpdateDto)
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
            device.ModifiedBy = deviceUpdateDto.ModifiedBy;
            device.IsPlanActive = device.EndDate >= DateTime.UtcNow ? true : false;

            try
            {
                await _context.SaveChangesAsync();
                //return Ok(device);
                return Ok(new
                {
                    Message = "Device is successfully updated!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //changes IsActive to false 
        [HttpPut("deleteDevice/{deviceId}")]
        public async Task<IActionResult> deleteDevice(String deviceId, DeviceDeleteDto deviceDeleteDto)
        {
            Device device = _context.Devices.Where(a => a.DeviceId == deviceId).FirstOrDefault();
            //var device = await _context.Devices.FindAsync(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            device.IsActive = false;
            device.ModifiedOn = DateTime.UtcNow;
            device.ModifiedBy = deviceDeleteDto.Modifiedby;
            try
            {
                await _context.SaveChangesAsync();
                //return Ok(device);
                return Ok(new
                {
                    Message = "Device is successfully deleted!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }





    }

}
