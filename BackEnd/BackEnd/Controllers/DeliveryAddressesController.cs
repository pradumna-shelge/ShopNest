using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using static BackEnd.DTOs.DelivaryAddressDto;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressesController : ControllerBase
    {
        private readonly ShopNestContext _context;

        public DeliveryAddressesController(ShopNestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method Name : GetDeliveryAddress()
        /// Purpose : This method is used to get delivery address to place order by user name
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>
        // GET: api/DeliveryAddresses/5
        [HttpGet("{name}")]
        public async Task<ActionResult> GetDeliveryAddress( string name)
        {

            if(name== null)
            {
                return NotFound();
            }

            var userObj = await _context.MstUsers.FirstOrDefaultAsync(u=>u.Username== name);
            if (userObj == null)
            {
                return NotFound();
            }

            var query = await _context.TrnUsersDeliveryAddresses
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


        /// <summary>
        /// Method Name : PutDeliveryAddress()
        /// Purpose : This method is used to update delivery address of user 
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>

        [HttpPut]
        public async Task<IActionResult> PutDeliveryAddress(AddressDTO obj)
        {
            try
            {
            if(obj == null)
            {
                return NotFound();
            }
            var newAddress = await _context.TrnUsersDeliveryAddresses.FirstOrDefaultAsync(d => d.AddressId == (obj.addressId??-1));
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


        /// <summary>
        /// Method Name : AddDeliveryAddress()
        /// Purpose : This method is used to add new Delivery address for specific user
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>

        [HttpPost]
        public async Task<ActionResult> AddDeliveryAddress(AddressDTO obj)
        {

            try
            {
                if(obj == null)
                {
                    return BadRequest();

                }
                var userObj = await _context.MstUsers.FirstOrDefaultAsync(u => u.Username == obj.username);
                if(userObj == null)
                {
                    return BadRequest();
                }
                var newAddress = new TrnUsersDeliveryAddress()
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

        /// <summary>
        /// Method Name : DeleteDeliveryAddress()
        /// Purpose : This method is used to remove delivery address by address id 
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryAddress(int id)
        {
            if (_context.TrnUsersDeliveryAddresses == null)
            {
                return NotFound();
            }
            var deliveryAddress = await _context.TrnUsersDeliveryAddresses.FindAsync(id);
            if (deliveryAddress == null)
            {
                return NotFound();
            }

            _context.TrnUsersDeliveryAddresses.Remove(deliveryAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
