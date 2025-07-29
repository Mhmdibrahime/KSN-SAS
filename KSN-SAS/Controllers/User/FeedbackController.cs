using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KSN_SAS.Models.Entities;
using KSN_SAS.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using KSN_SAS.Models.DTOs.FeedbackUserDTOs;
using KSN_SAS.Models.Data;

namespace KSN_SAS.Controllers.User
{
    [Route("User/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(AppDbContext context, ILogger<FeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackCreateDTO feedbackDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feedback = new Feedback
                {
                    Comment = feedbackDto.Comment,
                    Rating = feedbackDto.Rating,
                    Date = DateTime.UtcNow,
                    Status = "Pending", 
                    UserName = feedbackDto.UserName,
                    UserJob = feedbackDto.UserJob
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                var responseDto = new FeedbackResponseDTO
                {
                    Id = feedback.Id,
                    Comment = feedback.Comment,
                    Rating = feedback.Rating,
                    Date = feedback.Date,
                    Status = feedback.Status,
                    UserName = feedback.UserName,
                    UserJob = feedback.UserJob
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new feedback");
                return StatusCode(500, "An error occurred while submitting your feedback");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Where(f => f.Status == "Approved")
                    .OrderByDescending(f => f.Date)
                    .Select(f => new FeedbackResponseDTO
                    {
                        Id = f.Id,
                        Comment = f.Comment,
                        Rating = f.Rating,
                        Date = f.Date,
                        Status = f.Status,
                        UserName = f.UserName,
                        UserJob = f.UserJob
                    })
                    .ToListAsync();

                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedbacks");
                return StatusCode(500, "An error occurred while retrieving feedbacks");
            }
        }

    }
}