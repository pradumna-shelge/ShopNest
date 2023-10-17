using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.DTOs;
using static BackEnd.DTOs.DelivaryAddressDto;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressesController : ControllerBase
    {
        private readonly MyShoppingContext _context;

        public DeliveryAddressesController(MyShoppingContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryAddress>>> GetDeliveryAddresses()
        {
          if (_context.DeliveryAddresses == null)
          {
              return NotFound();
          }
            return await _context.DeliveryAddresses.ToListAsync();
        }

        // GET: api/DeliveryAddresses/5
        [HttpGet("{name}")]
        public async Task<ActionResult> GetDeliveryAddress( string name)
        {

            if(name== null)
            {
                return NotFound();
            }

            var userObj = await _context.Users.FirstOrDefaultAsync(u=>u.Username== name);
            if (userObj == null)
            {
                return NotFound();
            }

            var query = await _context.DeliveryAddresses
                .Select(a => new
                {
                    a.UserId,
                    a.AddressId,
                    country = a.Country,
                    state = a.State,
                    city = a.City,
                    firstName = a.FirstName,
                    lastName = a.LastName,
                    address = a.AddressLine,
                    zip = a.Zip
                }).Where(a=>a.UserId==userObj.UserId).OrderByDescending(a => a.AddressId)
                .ToListAsync();

            return Ok(query);
        }

     
        [HttpPut]
        public async Task<IActionResult> PutDeliveryAddress(AddressDTO obj)
        {
            try
            {
            if(obj == null)
            {
                return NotFound();
            }
            var newAddress = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.AddressId == (obj.addressId??-1));
            if(newAddress == null || obj.addressId==null)
            {
                return NotFound();
            }



            newAddress.FirstName = obj.firstName;
            newAddress.LastName = obj.lastName;
            newAddress.AddressId = (obj.addressId ?? -1);
            newAddress.Country = obj.country;
            newAddress.State = obj.state;
            newAddress.City = obj.city;
            newAddress.Zip = obj.zip;
                newAddress.AddressLine = obj.address;
            await _context.SaveChangesAsync();
            return Ok(newAddress);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryAddress>> PostDeliveryAddress(AddressDTO obj)
        {

            try
            {
                if(obj == null)
                {
                    return BadRequest();

                }
                var userObj = await _context.Users.FirstOrDefaultAsync(u => u.Username == obj.username);
                if(userObj == null)
                {
                    return BadRequest();
                }
                var newAddress = new DeliveryAddress()
                {
                    FirstName = obj.firstName,
                    LastName = obj.lastName,
                    AddressLine = obj.address,
                    City = obj.city,
                    Zip = obj.zip,
                    Country = obj.country,
                    State = obj.state,
                    UserId=userObj.UserId
                };

                await _context.AddAsync(newAddress);
                await _context.SaveChangesAsync();
                return Ok(newAddress);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/DeliveryAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryAddress(int id)
        {
            if (_context.DeliveryAddresses == null)
            {
                return NotFound();
            }
            var deliveryAddress = await _context.DeliveryAddresses.FindAsync(id);
            if (deliveryAddress == null)
            {
                return NotFound();
            }

            _context.DeliveryAddresses.Remove(deliveryAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryAddressExists(int id)
        {
            return (_context.DeliveryAddresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
