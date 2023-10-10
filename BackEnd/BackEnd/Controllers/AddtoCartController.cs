using BackEnd.DTOs;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddtoCartController : ControllerBase
    {

        private readonly MyShoppingContext _contex;
        private readonly IConfiguration _config;

        public AddtoCartController(MyShoppingContext context, IConfiguration configuration)
        {
            _contex = context;
            _config = configuration;
        }


        [HttpGet]

        public async Task<ActionResult> getCartData(string userName)
        {

           if(userName == null)
            {
                return BadRequest();
            }

           var userOb = await _contex.Users.FirstOrDefaultAsync(u=>u.Username== userName);
            if(userOb == null) {
                return BadRequest();

            }

            var data = await _contex.AddToCarts.Join(
                _contex.Products,
                cart => cart.ProductId,
                mapping => mapping.ProductId,
                (cart, mapping) => new        
                {
                    Price = $"{mapping.Price:0.00}",
                    Mrprice = $"{mapping.Mrprice:0.00}",
                    cart.Quantity,
                    mapping.ProductImage,
                    mapping.ProductName,
                    mapping.Description,
                    cart.CartId,
                    cart.UserId

                }).Where(cartMapping => cartMapping.UserId == userOb.UserId).OrderByDescending(x => x.CartId).ToListAsync();

         return Ok(data);
        }


        [HttpPost]
        public async Task<ActionResult> addCartData(cartDto cart)
        {
            try
            {

            var cartData = await _contex.AddToCarts.ToListAsync();
                var productData = await _contex.Products.ToListAsync();    
            
            if(cartData == null || productData == null)
            {
                    return BadRequest();
            }
                var productObj = productData.FirstOrDefault(p => p.ProductName == cart.productName);
                if(productObj != null)

                {
                    var userOb = await _contex.Users.FirstOrDefaultAsync(u => u.Username == cart.username);
                    if(userOb != null)
                    {
                    
                    var cartObj = cartData.FirstOrDefault(c => c.ProductId == productObj.ProductId && c.UserId == userOb.UserId);

                if(cartObj != null)
                {
                    cartObj.Quantity = cart.quantity;
                    cartObj.Price = cart.price;
                     
                        await _contex.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        var newCart = new AddToCart()
                        {
                            ProductId = productObj.ProductId,
                            Price = productObj.Price,
                            UserId = userOb.UserId,
                            Quantity = 1,
                            AddedDateTime= DateTime.Now
                        };
                     await   _contex.AddAsync(newCart);
                        await _contex.SaveChangesAsync();
                    }
                    }
                    return Ok();

                }
                else
                {
                    return BadRequest();
                }



            }
            catch (Exception ex) { 
             return BadRequest(ex);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartProduct(int id)
        {
            if (_contex.AddToCarts == null) { 
           
                return NotFound();
            }

            var product = await _contex.AddToCarts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _contex.AddToCarts.Remove(product);
            await _contex.SaveChangesAsync();

            return Ok("Product is Deleted");



        }





    }
}
