using ECommerce.Models;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("shop")]
    public class ShoppingController : ControllerBase
    {
        private readonly ILogger<ShoppingController> _logger;
        private readonly IShopService _shopService;

        public ShoppingController(ILogger<ShoppingController> logger, IShopService shopService)
        {
            _logger = logger;
            _shopService = shopService;
        }


        [HttpPost]
        [Route("user")]
        public async Task<ActionResult> AddUser(User user)
        {
            try
            {
                await _shopService.AddUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to add user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("cart")]
        public async Task<ActionResult> AddCart(Cart cart)
        {
            try
            {
                await _shopService.AddCart(cart);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to add cart: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("cart-item")]
        public async Task<ActionResult> AddItemToCart([FromBody]CartItem cartItem, int cartId)
        {
            try
            {
                var newCartItem = await _shopService.AddItemToCartAsync(cartItem, cartId);
                return Ok(newCartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to add cart item: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("cart-item")]
        public ActionResult GetCartItem(int cartItemId)
        {
            try
            {
                var cartItem = _shopService.GetCartItem(cartItemId);
                if (cartItem == null) return NotFound();

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to retrieve cart item: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("cart-items")]
        public ActionResult GetCartItems(int cartId)
        {
            try
            {
                var cartItems = _shopService.GetAllCartItems(cartId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to retrieve cart items: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("cart-item")]
        public ActionResult RemoveCartItem(int cartItemId)
        {
            try
            {
                var cartItem = _shopService.RemoveCartItem(cartItemId);
                if (cartItem == null) return NotFound();

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to delete cart item: {ex.Message}");
            }
        }
    }
}