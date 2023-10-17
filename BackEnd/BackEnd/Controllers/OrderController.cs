using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ShopNestContext _contex;

        public OrderController(ShopNestContext context)
        {
            _contex = context;
        }

        /// <summary>
        /// Method Name: PlaceOrder
        /// Purpose: Places an order for a user based on the contents of their cart.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string userName)
        {
            try
            {
                if (userName == null)
                {
                    return BadRequest();
                }

                var userOb = await _contex.MstUsers.FirstOrDefaultAsync(u => u.Username == userName);
                if (userOb == null)
                {
                    return BadRequest();
                }

                var filteredEntities = await _contex.TrnAddToCarts
                    .Where(data => data.UserId == userOb.UserId)
                    .ToListAsync();

                var newOrder = new TrnOrder()
                {
                    UserId = userOb.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = (decimal)(filteredEntities.Sum(u => u.Price))
                };

                await _contex.TrnOrders.AddAsync(newOrder);
                await _contex.SaveChangesAsync();

                foreach (var ob in filteredEntities)
                {
                    var newOrderItem = new TrnOrdersOrderItem()
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = ob.ProductId,
                        Quantity = ob.Quantity,
                        Price = (decimal)(ob.Price)
                    };

                    await _contex.TrnOrdersOrderItems.AddAsync(newOrderItem);
                    await _contex.SaveChangesAsync();
                }

                newOrder.InvoiceNo = GenerateRandomInvoiceNumber(newOrder.OrderId);

                _contex.RemoveRange(filteredEntities);
                await _contex.SaveChangesAsync();

                return Ok(new { invoke = newOrder.InvoiceNo });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Method Name: GenerateRandomInvoiceNumber
        /// Purpose: Generates a random invoice number based on the current date and order ID.
        /// Created By: Pradumna shelage
        /// Created Date: 17-10-2023
        /// Updated By:
        /// Updated Date:
        /// Updated Reason:
        /// </summary>
        private long GenerateRandomInvoiceNumber(int orderId)
        {
            string currentDateNumeric = DateTime.Now.ToString("yyyyMMdd");
            string invoiceNumberStr = $"{currentDateNumeric}{orderId}";

            if (long.TryParse(invoiceNumberStr, out long invoiceNumber))
            {
                return invoiceNumber;
            }
            else
            {
                throw new InvalidOperationException("Failed to generate a valid numeric invoice number.");
            }
        }
    }
}
