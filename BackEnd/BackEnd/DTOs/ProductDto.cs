namespace BackEnd.DTOs
{
    public class ProductDto
    {
 

        public string productName { get; set; }
   
         public  IFormFile productImage { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }

        public decimal mrp { get; set; }


    }
}
