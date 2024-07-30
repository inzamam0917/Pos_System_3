using Microsoft.AspNetCore.Mvc;
using Pos_System_3.Services;
using System.Threading.Tasks;
using Pos_System_3.Model;
using Pos_System_3.ApiModel;

namespace Pos_System_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _SaleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ISaleService SaleService, ILogger<SaleController> logger)
        {
            _SaleService = SaleService;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _SaleService.IsCashierAsync(token);
            _logger.LogInformation("Requesting Starting Sale...");
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can start a sale." });
            }

            var result = await _SaleService.StartNewSaleAsync(token);
            if (result)
            {
                return Ok(new { message = "Sale started successfully" });
            }
            return BadRequest(new { message = "Unable to start sale" });
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProductToSale([FromHeader] string Authorization, [FromBody] SaleProductDTO model)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _SaleService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can add products to a sale." });
            }

            var result = await _SaleService.AddProductToSaleAsync(model.ProductId, model.Quantity, token);
            if (result)
            {
                return Ok(new { message = "Product added to sale successfully" });
            }
            return BadRequest(new { message = "Unable to add product to sale" });
        }

        [HttpPost("removeproduct")]
        public async Task<IActionResult> RemoveProductFromSale([FromHeader] string Authorization, [FromBody] SaleProductDTO model)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _SaleService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can remove products from a sale." });
            }

            var result = await _SaleService.RemoveProductFromSaleAsync(model.ProductId, model.Quantity, token);
            if (result)
            {
                return Ok(new { message = "Product removed from sale successfully" });
            }
            return BadRequest(new { message = "Unable to remove product from sale" });
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _SaleService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can complete a sale." });
            }

            var result = await _SaleService.CompleteSaleAsync(token);
            if (result)
            {
                return Ok(new { message = "Sale completed successfully" });
            }
            return BadRequest(new { message = "Unable to complete sale" });
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _SaleService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can view the current sale." });
            }

            var sale = _SaleService.GetCurrentSale();
            if (sale != null)
            {
                return Ok(sale);
            }
            return NotFound(new { message = "No sale in progress" });
        }
    }
}
