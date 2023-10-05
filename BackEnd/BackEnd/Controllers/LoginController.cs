using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using NuGet.Versioning;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyShoppingContext _contex;
        private readonly IConfiguration _config;

        public LoginController(MyShoppingContext context,IConfiguration configuration)
        {
                _contex = context;
            _config = configuration;
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> forgotPassword(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) { return BadRequest("Please enter your name"); };

                var userObject = await _contex.Users.FirstOrDefaultAsync(u => u.Username == userName.ToLower());

                if(userObject != null) {
                 
                    var currDate = DateTime.Now;
                    

                    if((userObject.ResetLinkExpiration != null && userObject.ResetLinkExpiration.Value.AddMinutes(5) < currDate) || userObject.ResetLinkExpiration == null)
                    {
                        userObject.ResetLinkExpiration= DateTime.Now;
                        userObject.ResetLink = "";
                    }
                  await  _contex.SaveChangesAsync();

                
                }
                    return Ok("Please check your email to Reset Password");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> login(signInDTO obj)
        {
                var users = await _contex.Users.ToListAsync();

            try
            {
                if(obj == null) {
                    return BadRequest("Invaild object");
                }
                string key = _config["secret-key:login"] ?? "not found";

                if(key == "not found")
                {
                    return NotFound("Incorrect username or password");
                }



                string decryptedUsername = Decrypt(obj.username, key);
                string decryptedPassword = Decrypt(obj.password, key);


                var userExitflag = users.Find(u => u.Username == decryptedUsername);

                if(userExitflag == null)
                {
                    return NotFound("Incorrect username or password");
                }

                //HashPasswordClass.VerifyPassword(decryptedPassword, userExitflag.PasswordHash)

                if (userExitflag.Username==decryptedUsername && userExitflag.PasswordHash==decryptedPassword) {
                    Token token = new Token();
                    var userInfo = new
                    {
                        pcno = userExitflag.LoginPcno,
                        lastLogin = userExitflag.LastLogin
                    };
                    userExitflag.LastLogin= DateTime.Now;
                    userExitflag.LoginPcno = GetLocalIPAddress();
                    ;
                    await _contex.SaveChangesAsync();

                    var role =await _contex.UserRoleMappings.FirstOrDefaultAsync(f=>f.UserId== userExitflag.UserId);
                    var roleName = "";
                    if (role != null)
                    {

                     var rolesList = await _contex.UserRoles.FirstOrDefaultAsync(f => f.RoleId == role.RoleId);
                        roleName = rolesList.RoleName;
                    }
                    var Token = token.createToken(userExitflag.Username, roleName);
                return Ok(new { token = Token, userInfo });
                       
                }
                    return NotFound("Incorrect username or password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string Decrypt(string encryptedData, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        private string GetLocalIPAddress()
        {
            string localIP = "";

            
            string hostName = Dns.GetHostName();

           
            IPAddress[] localIPAddresses = Dns.GetHostAddresses(hostName);

           
            foreach (IPAddress ipAddress in localIPAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ipAddress.ToString();
                    break;
                }
            }

            return localIP;
        }

    }

}
