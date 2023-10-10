using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly MyShoppingContext _contex;

        public OrderController(MyShoppingContext context)
        {
            _contex = context;
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string userName)
        {
            try
            {
                    if (userName == null)
                {
                    return BadRequest();
                }

                var userOb = await _contex.Users.FirstOrDefaultAsync(u => u.Username == userName);
                if (userOb == null)
                {
                    return BadRequest();

                }


                var filteredEntities = await _contex.AddToCarts
        .Where(data => data.UserId == userOb.UserId)
        .ToListAsync();

                var newOrder = new Order()
                {
                    UserId = userOb.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = (decimal)(filteredEntities.Sum(u => u.Price) ?? 0)
                };

                await _contex.Orders.AddAsync(newOrder);
                await _contex.SaveChangesAsync();


                foreach (var ob in filteredEntities)
                {

                    var newOrderItem = new OrderItem()
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = ob.ProductId,
                        Quantity = ob.Quantity,
                        Price = (decimal)(ob.Price ?? 0)
                    };

                    await _contex.OrderItems.AddAsync(newOrderItem);

                    await _contex.SaveChangesAsync();

                }

                newOrder.InvoiceNo = GenerateRandomInvoiceNumber(newOrder.OrderId);

                _contex.RemoveRange(filteredEntities);
                await _contex.SaveChangesAsync();

                return Ok(new { invoke=newOrder.InvoiceNo });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

          

        }

        private long GenerateRandomInvoiceNumber(int orderId)
        {
            // Get the current date and time as a numeric string
            string currentDateNumeric = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Combine the order ID and current date numeric string
            string invoiceNumberStr = $"{orderId}{currentDateNumeric}";

            // Convert the combined string to a long integer
            if (long.TryParse(invoiceNumberStr, out long invoiceNumber))
            {
                return invoiceNumber;
            }
            else
            {
                // Handle the case where the conversion fails
                throw new InvalidOperationException("Failed to generate a valid numeric invoice number.");
            }
        }




    }
}
