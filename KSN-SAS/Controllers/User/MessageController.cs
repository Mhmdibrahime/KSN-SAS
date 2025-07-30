using KSN_SAS.Models.Data;
using KSN_SAS.Models.DTOs.MessageAdminDTOs;
using KSN_SAS.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSN_SAS.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext context;

        public MessageController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult AddMessage(MessageDto dto)
        {
            if (ModelState.IsValid)
            {
                var Message = new Message()
                {
                    Name = dto.Name,
                    Phone = dto.Phone,
                    Description = dto.Description
                };
                context.Messages.Add(Message);
                context.SaveChanges();
                return Ok("Sent");
            }
            else
            {
                return BadRequest("An error occurred while submitting your Message");
            }

          
        }
    }
}
