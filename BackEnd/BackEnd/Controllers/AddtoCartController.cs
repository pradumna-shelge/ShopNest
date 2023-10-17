using BackEnd.DTOs;
using BackEnd.DTOs.spDTO;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddtoCartController : ControllerBase
    {

        private readonly ShopNestContext _context;
        private readonly IConfiguration _config;

        public AddtoCartController(ShopNestContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        /// <summary>
        /// Method Name : getCartData()
        /// Purpose : This method is used to get user shopping cart data by name
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>

        [HttpGet]

        public async Task<ActionResult> getCartData(string userName)
        {
            try
            {
                if (userName == null)
                {
                    return BadRequest("UserName cannot be null");
                }

                var userNameParam = new SqlParameter("@UserName", userName);

                
                var data = await _context
                    .Set<CartItemDTO>()
                    .FromSqlRaw("EXEC GetCartData @UserName", userNameParam)
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }


        /// <summary>
        /// Method Name : addCartData()
        /// Purpose : This method is used to add new item in user cart 
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> addCartData(cartDto cart)
        {
            try
            {
                var cartData = await _context.TrnAddToCarts.ToListAsync();
                var productData = await _context.MstProducts.ToListAsync();

                if (cartData == null || productData == null)
                {
                    return BadRequest("Cart or product data is not available.");
                }

                var productObj = productData.FirstOrDefault(p => p.ProductName == cart.productName);
                if (productObj != null)
                {
                    var userOb = await _context.MstUsers.FirstOrDefaultAsync(u => u.Username == cart.username);
                    if (userOb != null)
                    {
                        var cartObj = cartData.FirstOrDefault(c => c.ProductId == productObj.ProductId && c.UserId == userOb.UserId);

                        if (cartObj != null)
                        {
                            cartObj.Quantity = cart.quantity;
                            cartObj.Price = cart.price;

                            await _context.SaveChangesAsync();

                            return Ok("Cart updated successfully.");
                        }
                        else
                        {
                            var newCart = new TrnAddToCart()
                            {
                                ProductId = productObj.ProductId,
                                Price = productObj.Price,
                                UserId = userOb.UserId,
                                Quantity = 1,
                                AddedDateTime = DateTime.Now // It is fro product added date
                            };

                            await _context.AddAsync(newCart);
                            await _context.SaveChangesAsync();

                            return Ok("Cart added successfully.");
                        }
                    }
                    else
                    {
                        return BadRequest("User not found.");
                    }
                }
                else
                {
                    return BadRequest("Product not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        
    }

        /// <summary>
        /// Method Name : DeleteCartProduct()
        /// Purpose : This method is used to remove cart item by cart item id 
        /// Created By : Pradumana shelage
        /// Created Date : 17/10/2023
        /// Updated By :
        /// Updated Date :
        /// Updated Reason :
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartProduct(int id)
        {
            try
            {
                var product = await _context.TrnAddToCarts.FindAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                _context.TrnAddToCarts.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Product is Deleted");
            }
            catch (Exception ex)
            {
                // Handle and log the exception as needed.
                return StatusCode(500, "An error occurred: " + ex.Message);
            }



        }





    }
}
