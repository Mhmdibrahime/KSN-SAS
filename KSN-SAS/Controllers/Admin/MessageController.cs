using KSN_SAS.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSN_SAS.Controllers.Admin
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext context;

        public MessageController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet("Messages")]
        public IActionResult GetAllMessages()
        {
            var Messages = context.Messages.ToList();
            return Ok(Messages);
        }
        [HttpDelete("{Id}")]
        public IActionResult DeleteMessages(int Id)
        {
            var Message = context.Messages.FirstOrDefault(m=>m.Id==Id);
            context.Messages.Remove(Message);
            context.SaveChanges();
            return Ok("Deleted");
        }
    }
}
