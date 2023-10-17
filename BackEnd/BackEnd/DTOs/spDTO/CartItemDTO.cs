using Microsoft.EntityFrameworkCore;

namespace BackEnd.DTOs.spDTO
{
        [Keyless]
    public class CartItemDTO
    {
        public int CartId { get; set; }
        public decimal Price { get; set; }
        public decimal Mrprice { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
