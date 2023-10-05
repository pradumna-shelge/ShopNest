using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.DTOs;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using System.Net;
using NuGet.Packaging;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyShoppingContext _context;
        private readonly IConfiguration _configuration;

        public ProductsController(MyShoppingContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var productNames = await _context.Products
      .Select(x => new { x.ProductName,x.ProductId,
          Price = $"{x.Price:0.00}",
          Mrprice = $"{x.Mrprice:0.00}", x.Description,x.ProductImage}).OrderByDescending(x => x.ProductId)
      .ToListAsync();

            return Ok(productNames);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromForm] UpdateProductDto product)
        {
            if (product==null)
            {
                return BadRequest();
            }
            try
            {
                var check = await _context.Products.FirstOrDefaultAsync(p => p.ProductName.ToLower().Trim() == product.productName.ToLower().Trim() && p.ProductId!=product.productId);
                if (check != null)
                {
                    return BadRequest("Product name already exist");
                }

                var  ob = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.productId);
            if (ob == null)
            {
                return BadRequest();
            }
             

            var imageUrl = ob.ProductImage;

                if (product.productImage != null)
                {
                    IFormFile file = product.productImage;
                    if (file == null || file.Length == 0)
                    {

                        return BadRequest("No file uploaded.");
                    }

                    const long maxFileSize = 2 * 1024 * 1024;
                    if (file.Length > maxFileSize)
                    {

                        return BadRequest("File size exceeds 2MB limit.");
                    }


                    if (!IsSupportedFileType(file.ContentType) || !file.ContentType.StartsWith("image/"))
                    {
                        return BadRequest("Invalid file type. Supported types are png, jpg, jpeg, bmp.");
                    }

                    string accessKey = _configuration["AWS:AccessKey"] ?? "No Data";
                    string secretKey = _configuration["AWS:SecretKey"] ?? "No Data";
                    string bucketName = _configuration["AWS:BucketName"] ?? "No Data";

                    if ((accessKey == "No Data") || (secretKey == "No Data") || (bucketName) == "No Data")
                    {
                        return BadRequest("credentials Not Found");
                    }


                    if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(bucketName))
                        return StatusCode(500, "AWS credentials or bucket configuration missing.");

                    using (var client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSouth1))
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        var fileTransferUtility = new TransferUtility(client);
                        var keyName = Guid.NewGuid().ToString();
                        await fileTransferUtility.UploadAsync(memoryStream, bucketName, keyName);

                        var urlRequest = new GetPreSignedUrlRequest
                        {
                            BucketName = bucketName,
                            Key = keyName,
                            Expires = DateTime.Now.AddDays(6)
                        };

                        string v = client.GetPreSignedURL(urlRequest);
                        imageUrl = v;


                    }
                    




                }



                ob.ProductImage = imageUrl;
                ob.ProductName = product.productName;
                ob.Price = AddDecimalIfMissing( product.price);
                ob.ProductId = product.productId;
                ob.Description = product.description;

                ob.Mrprice = AddDecimalIfMissing( product.mrp);

                
                await _context.SaveChangesAsync();

                //return Ok("updated successfuly");

                return Ok(ob);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(ProductDto product)
        //{
        //    try
        //    {
        //        if (_context.Products == null)
        //        {
        //            return Problem("Entity set 'MyShoppingContext.Products'  is null.");
        //        }

        //        Product product1 = new Product()
        //        {
        //            ProductName = product.productName,
        //            ProductImage = product.productImage,
        //            Description = product.description,
        //            Price = product.price
        //        };
        //        _context.Products.Add(product1);
        //        await _context.SaveChangesAsync();

        //        return Ok("added data");

        //    }
        //    catch(DbUpdateConcurrencyException)
        //    {
        //        return BadRequest("something went wrong");
        //    }
          
        //}



        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct1([FromForm] ProductDto product)
        {
            try
            {
                if (_context.Products == null)
                {
                    return Problem("Entity set 'MyShoppingContext.Products'  is null.");
                }
                var ob = await _context.Products.FirstOrDefaultAsync(p=>p.ProductName.ToLower().Trim() == product.productName.ToLower().Trim());
                if (ob != null)
                {
                    return BadRequest("Product name already exist");
                }
                IFormFile file = product.productImage;
                if (file == null || file.Length == 0)
                {

                    return BadRequest("No file uploaded.");
                }

                const long maxFileSize = 2 * 1024 * 1024;
                if (file.Length > maxFileSize)
                {

                    return BadRequest("File size exceeds 2MB limit.");
                }


                if (!IsSupportedFileType(file.ContentType) || !file.ContentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid file type. Supported types are png, jpg, jpeg, bmp.");
                }

                string accessKey = _configuration["AWS:AccessKey"] ?? "No Data";
                string secretKey = _configuration["AWS:SecretKey"] ?? "No Data";
                string bucketName = _configuration["AWS:BucketName"] ?? "No Data";

                if ((accessKey == "No Data") || (secretKey == "No Data") || (bucketName) == "No Data")
                {
                    return BadRequest("credentials Not Found");
                }


                if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(bucketName))
                    return StatusCode(500, "AWS credentials or bucket configuration missing.");

                using (var client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSouth1))
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var fileTransferUtility = new TransferUtility(client);
                    var keyName = Guid.NewGuid().ToString();
                    await fileTransferUtility.UploadAsync(memoryStream, bucketName, keyName);

                    var urlRequest = new GetPreSignedUrlRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        Expires = DateTime.Now.AddDays(6)
                    };

                    string imageUrl = client.GetPreSignedURL(urlRequest);
                    ;

                    Product product1 = new Product()
                    {
                        ProductName = product.productName.Trim(),
                        ProductImage = imageUrl,
                        Description = product.description.Trim(),
                        Price = AddDecimalIfMissing( product.price),
                        Mrprice = AddDecimalIfMissing( product.mrp)
                        
                    };
                    _context.Products.Add(product1);
                    await _context.SaveChangesAsync();

                    return Ok("added data");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("something went wrong");
            }

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cartProducts = await _context.AddToCarts
    .Where(c => c.ProductId == product.ProductId )
    .ToListAsync();
           

            if (cartProducts.Any())
            {
                _context.RemoveRange(cartProducts);
                await _context.SaveChangesAsync();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product is Deleted");



        }




        private decimal AddDecimalIfMissing(decimal input)
        {
            
            string formattedString = input.ToString("0.00");

           
            decimal formattedDecimal;
            if (decimal.TryParse(formattedString, out formattedDecimal))
            {
                return formattedDecimal;
            }
            else
            {
                
                throw new ArgumentException("Invalid decimal format");
            }
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        private bool IsSupportedFileType(string contentType)
        {

            string[] supportedTypes = { "image/png", "image/jpeg", "image/jpg", "image/bmp" };

            return supportedTypes.Contains(contentType.ToLower());
        }
    }
}
