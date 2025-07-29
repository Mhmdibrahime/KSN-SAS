using System.ComponentModel.DataAnnotations;

namespace KSN_SAS.Models.DTOs.FeedbackUserDTOs
{
    public class FeedbackCreateDTO
    {
        [Required, MaxLength(10000)]
        public string Comment { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string UserJob { get; set; }
    }
}