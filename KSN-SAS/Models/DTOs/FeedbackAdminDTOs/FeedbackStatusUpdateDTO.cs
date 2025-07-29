using System.ComponentModel.DataAnnotations;

namespace KSN_SAS.Models.DTOs.FeedbackAdminDTOs
{
    public class FeedbackStatusUpdateDTO
        {
            [Required]
            public string Status { get; set; } // "Approved", "Rejected", or "Pending"
        }
    
}
