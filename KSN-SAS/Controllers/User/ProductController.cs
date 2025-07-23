using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSN_SAS.Controllers.User
{
    [Route("User/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetProducts()
        {
            return Ok(new { ProductId = 1 });
        }
    }
}
