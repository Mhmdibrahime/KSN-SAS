using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KSN_SAS.Models.Entities;
using KSN_SAS.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using KSN_SAS.Models.DTOs.FeedbackAdminDTOs;
using KSN_SAS.Models.Data;

namespace KSN_SAS.Controllers.Admin
{
    [Route("Admin/[controller]")]
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Select(f => new FeedbackDTO
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
                _logger.LogError(ex, "Error retrieving all feedbacks");
                return StatusCode(500, "An error occurred while retrieving feedbacks");
            }
        }

        [HttpGet("GetApproved")]
        public async Task<IActionResult> GetApproved()
        {
            try
            {
                var approvedFeedbacks = await _context.Feedbacks
                    .Where(f => f.Status == "Approved")
                    .Select(f => new FeedbackDTO
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

                return Ok(approvedFeedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving approved feedbacks");
                return StatusCode(500, "An error occurred while retrieving approved feedbacks");
            }
        }

        [HttpGet("GetRejected")]
        public async Task<IActionResult> GetRejected()
        {
            try
            {
                var rejectedFeedbacks = await _context.Feedbacks
                    .Where(f => f.Status == "Rejected")
                    .Select(f => new FeedbackDTO
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

                return Ok(rejectedFeedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rejected feedbacks");
                return StatusCode(500, "An error occurred while retrieving rejected feedbacks");
            }
        }

        [HttpGet("GetPending")]
        public async Task<IActionResult> GetPending()
        {
            try
            {
                var pendingFeedbacks = await _context.Feedbacks
                    .Where(f => f.Status == "Pending")
                    .Select(f => new FeedbackDTO
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

                return Ok(pendingFeedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending feedbacks");
                return StatusCode(500, "An error occurred while retrieving pending feedbacks");
            }
        }

        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound($"Feedback with ID {id} not found");
                }

                feedback.Status = "Approved";
                await _context.SaveChangesAsync();

                return Ok(new FeedbackDTO
                {
                    Id = feedback.Id,
                    Comment = feedback.Comment,
                    Rating = feedback.Rating,
                    Date = feedback.Date,
                    Status = feedback.Status,
                    UserName = feedback.UserName,
                    UserJob = feedback.UserJob
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error accepting feedback with ID {id}");
                return StatusCode(500, $"An error occurred while accepting feedback with ID {id}");
            }
        }

        [HttpPost("Reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound($"Feedback with ID {id} not found");
                }

                feedback.Status = "Rejected";
                await _context.SaveChangesAsync();

                return Ok(new FeedbackDTO
                {
                    Id = feedback.Id,
                    Comment = feedback.Comment,
                    Rating = feedback.Rating,
                    Date = feedback.Date,
                    Status = feedback.Status,
                    UserName = feedback.UserName,
                    UserJob = feedback.UserJob
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting feedback with ID {id}");
                return StatusCode(500, $"An error occurred while rejecting feedback with ID {id}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound($"Feedback with ID {id} not found");
                }

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting feedback with ID {id}");
                return StatusCode(500, $"An error occurred while deleting feedback with ID {id}");
            }
        }
    }
}