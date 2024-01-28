using ECommSiteApis.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite.Models;
using Site.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ShoppingSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _context.Products.ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("{productId}")]
        public IActionResult GetProduct(int productId)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

                if (product == null)
                {
                    return NotFound(); 
                }

                var productDto = MapProductToDto(product);

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        private ProductDto MapProductToDto(Products product)
        {
            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Color=product.Color,
                Description=product.Description
               
            };
        }
        [HttpPost("addToCart")]
        public IActionResult AddToCart([FromBody] AddORemoveCartRequest model)
        {
            try
            {
               
                var product = _context.Products.Find(model.ProductId);
                if (product == null)
                {
                    return BadRequest("Product not found");
                }

               
                var user = _context.Users.Find(model.UserId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                
                var existingCartItem = _context.CartItems
                    .SingleOrDefault(c => c.UserId == model.UserId && c.ProductId == model.ProductId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += model.Quantity;
                }
                else
                {
                   
                    var newCartItem = new CartItems
                    {
                        UserId = model.UserId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                    };

                    _context.CartItems.Add(newCartItem);
                }

               
                _context.SaveChanges();

                return Ok("Product added to the cart");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("updateCartItemQuantity")]
        public IActionResult UpdateCartItemQuantity([FromBody] AddORemoveCartRequest model)
        {
            try
            {
                var existingCartItem = _context.CartItems
                    .SingleOrDefault(c => c.UserId == model.UserId && c.ProductId == model.ProductId);

                if (existingCartItem != null)
                {
                    
                    existingCartItem.Quantity = Math.Max(0, existingCartItem.Quantity - model.Quantity);

                    _context.SaveChanges();
                    return Ok("Cart item quantity updated");
                }

                return BadRequest("Cart item not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }




        [HttpGet("getCart")]
        public IActionResult GetCart([FromQuery] int userId)
        {
            try
            {
                var cartItemsWithProducts = _context.CartItems
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)  
                    .Select(cartItem => new
                    {
                        cartItem.ProductId,
                        cartItem.Quantity,
                        cartItem.Product.Name,
                        cartItem.Product.ImageUrl,
                        cartItem.Product.Color,
                        cartItem.Product.Price,
                        cartItem.Product.Description
                    })
                    .ToList();

                return Ok(cartItemsWithProducts);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }





        [HttpDelete("removeFromCart/{productId}")]
        public IActionResult RemoveFromCart(int productId, [FromQuery] int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("Invalid user ID");
                }

              
                var cartItem = _context.CartItems
                    .SingleOrDefault(c => c.UserId == userId && c.ProductId == productId);

                if (cartItem != null)
                {
                    
                    _context.CartItems.Remove(cartItem);
                    _context.SaveChanges();

                    return Ok("Item removed from the cart");
                }

               
                return NotFound("Item not found in the cart");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }










    }

}
