using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models;

namespace WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public CheckoutController(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        [HttpGet("GetCheckout")]
        public async Task<IActionResult> GetCheckout()
        {
            var check = await _checkoutRepository.GetCheckout();

            if (check == null)
            {
                return NotFound();
            }

            return Ok(check);
        }


        [HttpPost("AddCheckoutWeb")]
        public async Task<IActionResult> AddCheckout(CheckoutDTO checkoutDto)
        {

            var check = await _checkoutRepository.ProcessCheckout(checkoutDto);

            return Ok(check);
        }

        [HttpGet("ResponseCheckout/{id}")]
        public async Task<IActionResult> ResponseCheckout(Guid id)
        {
            var check = await _checkoutRepository.GetCheckoutById(id);
            if (check == null)
            {
                return NotFound();
            }

            return Ok(check);
        }

        [HttpPut("SoftDeleteCheckout/{id}")]
        public async Task<IActionResult> SoftDeleteCheckout(Guid id)
        {
            await _checkoutRepository.SoftDeleteCheckout(id);

            return Ok( new { message = "Done"} );
        }
    }
}