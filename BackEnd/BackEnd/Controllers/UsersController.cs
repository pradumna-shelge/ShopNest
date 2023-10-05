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
        private readonly MyShoppingContext _context;

        public UsersController(MyShoppingContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
          //var data = await _context.Users.Select(u => new { u.Username, u.Email, u.LastLogin, u.UserId }).OrderByDescending(u=>u.UserId).ToListAsync();
            var data = await _context.Users
    .Join(
        _context.UserRoleMappings,
        user => user.UserId,
        mapping => mapping.UserId,
        (user, mapping) => new
        {
            user.Username,
            user.Email,
            user.LastLogin,
            user.UserId,
            mapping.RoleId ,
            user.PasswordHash
        })
    .OrderByDescending(u => u.UserId)
    .ToListAsync();
            return Ok(data);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser(userDto user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                var check = await _context.Users.FirstOrDefaultAsync(p => p.Username.ToLower().Trim() == user.Username.ToLower().Trim() && p.UserId != user.userId);
                if (check != null)
                {
                    return BadRequest("That username is taken.Try another");
                }
                var checkEmail = await _context.Users.FirstOrDefaultAsync(p => p.Email.ToLower().Trim() == user.Email.ToLower().Trim() && p.UserId != user.userId);
                if (checkEmail != null)
                {
                    return BadRequest("Email is taken.Use another");
                }

                var ob = await _context.Users.FirstOrDefaultAsync(p => p.UserId == user.userId);
                if (ob == null)
                {
                    return BadRequest();
                }


               

                ob.Username = user.Username;
                ob.Email = user.Email;

                //ob.PasswordHash = ob.PasswordHash != "user-password" ?   HashPasswordClass.HashPassword(user.PasswordHash):ob.PasswordHash;
                ob.PasswordHash = user.PasswordHash;
                var mapID = await _context.UserRoleMappings.FirstOrDefaultAsync(m => m.UserId == ob.UserId);
                var mapOb = await _context.UserRoleMappings.FindAsync(mapID !=null ? mapID.MappingId:-1);

                if(mapOb != null)
                {
                    mapOb.RoleId = user.userRole != null ? user.userRole : 2;
                    await _context.SaveChangesAsync();

                }


                await _context.SaveChangesAsync();

                //return Ok("updated successfuly");

                return Ok(ob);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            
        }


        [HttpPost]
        public async Task<ActionResult> PostUser(userDto user)
        {
            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'MyShoppingContext.Users'  is null.");
                }
                var users = await _context.Users.ToListAsync();

                if(users.Find(p => p.Username.ToLower().Trim() == user.Username.ToLower().Trim()) != null)
                {
                    return BadRequest("That username is taken.Try another");
                }

                var checkEmail = await _context.Users.FirstOrDefaultAsync(p => p.Email.ToLower().Trim() == user.Email.ToLower().Trim() && p.UserId != user.userId);
                if (checkEmail != null)
                {
                    return BadRequest("Email is taken.Use another");
                }
                //user.PasswordHash = HashPasswordClass.HashPassword(user.PasswordHash);


                var newUser = new User()
                {
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate,
                    LastLogin = DateTime.Now,
                    
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                var listUsers = await _context.Users.ToListAsync();
                var usermappin = new UserRoleMapping
                {
                    UserId = newUser.UserId,
                    RoleId =  listUsers.ToList().Count()<2 || user.userRole==1   ? 1: 2
                };
                await _context.UserRoleMappings.AddAsync(usermappin);
                await _context.SaveChangesAsync();

                return Ok("User added");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
          
          
        }

        // DELETE: api/Users/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRole = await _context.UserRoleMappings.FirstOrDefaultAsync(m => m.UserId == user.UserId);
            var usermap = await _context.UserRoleMappings.FindAsync(userRole.MappingId);

            var cartProducts = await _context.AddToCarts.Where(c => c.UserId == user.UserId).ToListAsync();

            if(cartProducts.Any())
            {
                _context.RemoveRange(cartProducts);
                await _context.SaveChangesAsync();
            }
            if (usermap != null)
            {
                _context.UserRoleMappings.Remove(usermap);
                await _context.SaveChangesAsync();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User is Deleted");
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        

    }
}
