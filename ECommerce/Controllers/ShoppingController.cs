using ECommerce.Models;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("shop")]
public class ShoppingController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShoppingController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpPost]
    [Route("cart-item")]
    public async Task<ActionResult> AddItemToCart([FromBody] CartItem cartItem, string phoneNumber, int cartId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "PhoneNumber must be 10 digits long");
            }

            var newCartItem = await _shopService.AddItemToCartAsync(cartItem, phoneNumber, cartId);
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
    [Route("filter-cart-items")]
    public ActionResult<List<CartItem>> GetCartItemsWithFilters(string phoneNumber, DateTime time, int quantity, int itemId)
    {
        try
        {
            var cartItems = _shopService.GetCartItems(
                phoneNumber, time, quantity, itemId);
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