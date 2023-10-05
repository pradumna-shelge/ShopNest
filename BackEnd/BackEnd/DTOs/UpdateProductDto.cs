namespace BackEnd.DTOs
{
    public class UpdateProductDto
    {
        public int productId { get; set; }
        public string productName { get; set; }

        public IFormFile? productImage { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public decimal mrp { get; set; }
        //public bool IsproductImageUpadte { get; set; }  = false;


    }
}
