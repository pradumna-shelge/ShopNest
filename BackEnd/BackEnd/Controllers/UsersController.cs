using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using System.Security.Cryptography;
using BackEnd.DTOs;
using BackEnd.Services;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShopNestContext _context;

        public UsersController(ShopNestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method Name: GetUsers
        /// Purpose: Retrieves a list of user information.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            if (_context.MstUsers == null)
            {
                return NotFound();
            }
            var data = await _context.VwUserInfos
                .ToListAsync();
            return Ok(data);
        }

        /// <summary>
        /// Method Name: PutUser
        /// Purpose: Updates user information with the provided data.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> PutUser(userDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                var check = await _context.MstUsers.FirstOrDefaultAsync(p => p.Username.ToLower().Trim() == user.Username.ToLower().Trim() && p.UserId != user.userId);
                if (check != null)
                {
                    return BadRequest("That username is taken. Try another.");
                }

                var checkEmail = await _context.MstUsers.FirstOrDefaultAsync(p => p.Email.ToLower().Trim() == user.Email.ToLower().Trim() && p.UserId != user.userId);
                if (checkEmail != null)
                {
                    return BadRequest("Email is taken. Use another.");
                }

                var ob = await _context.MstUsers.FirstOrDefaultAsync(p => p.UserId == user.userId);
                if (ob == null)
                {
                    return BadRequest();
                }

                ob.Username = user.Username;
                ob.Email = user.Email;
                ob.PasswordHash = user.PasswordHash;

                var mapID = await _context.TrnUserRoleMappings.FirstOrDefaultAsync(m => m.UserId == ob.UserId);
                var mapOb = await _context.TrnUserRoleMappings.FindAsync(mapID != null ? mapID.MappingId : -1);

                if (mapOb != null)
                {
                    mapOb.RoleId = user.userRole != null ? user.userRole : 2;
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();

                return Ok(ob);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Method Name: PostUser
        /// Purpose: Adds a new user with the provided data.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostUser(userDto user)
        {
            try
            {
                if (_context.MstUsers == null)
                {
                    return Problem("Entity set 'MyShoppingContext.Users' is null.");
                }

                var users = await _context.MstUsers.ToListAsync();

                if (users.Find(p => p.Username.ToLower().Trim() == user.Username.ToLower().Trim()) != null)
                {
                    return BadRequest("That username is taken. Try another.");
                }

                var checkEmail = await _context.MstUsers.FirstOrDefaultAsync(p => p.Email.ToLower().Trim() == user.Email.ToLower().Trim() && p.UserId != user.userId);
                if (checkEmail != null)
                {
                    return BadRequest("Email is taken. Use another.");
                }

                var newUser = new MstUser()
                {
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate,
                    LastLogin = DateTime.Now,
                };

                await _context.MstUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();

                var listUsers = await _context.MstUsers.ToListAsync();
                var userMapping = new TrnUserRoleMapping()
                {
                    UserId = newUser.UserId,
                    RoleId = listUsers.Count() < 2 || user.userRole == 1 ? 1 : 2
                };
                await _context.TrnUserRoleMappings.AddAsync(userMapping);
                await _context.SaveChangesAsync();

                return Ok("User added");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Method Name: DeleteUser
        /// Purpose: Deletes a user and related data by their ID.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.MstUsers == null)
            {
                return NotFound();
            }

            var user = await _context.MstUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = await _context.TrnUserRoleMappings.FirstOrDefaultAsync(m => m.UserId == user.UserId);
            var userMapping = await _context.TrnUserRoleMappings.FindAsync(userRole.MappingId);
            var cartProducts = await _context.TrnAddToCarts.Where(c => c.UserId == user.UserId).ToListAsync();
            var orders = await _context.TrnOrders.Where(o => o.UserId == user.UserId).ToListAsync();
            var address = await _context.TrnUsersDeliveryAddresses.Where(o => o.UserId == user.UserId).ToListAsync();

            if (orders.Any())
            {
                foreach (var ob in orders)
                {
                    var orderItems = await _context.TrnOrdersOrderItems.Where(or => or.OrderId == ob.OrderId).ToListAsync();
                    _context.TrnOrdersOrderItems.RemoveRange(orderItems);
                    await _context.SaveChangesAsync();
                }
                _context.RemoveRange(orders);
                await _context.SaveChangesAsync();
            }

            if (address.Any())
            {
                _context.RemoveRange(address);
                await _context.SaveChangesAsync();
            }

            if (cartProducts.Any())
            {
                _context.RemoveRange(cartProducts);
                await _context.SaveChangesAsync();
            }

            if (userMapping != null)
            {
                _context.TrnUserRoleMappings.Remove(userMapping);
                await _context.SaveChangesAsync();
            }

            _context.MstUsers.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User is Deleted");
        }

        private bool UserExists(int id)
        {
            return (_context.MstUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
