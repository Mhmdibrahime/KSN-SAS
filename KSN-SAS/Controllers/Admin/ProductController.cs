using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSN_SAS.Controllers.Admin
{
    [Route("Admin/[controller]")]
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
