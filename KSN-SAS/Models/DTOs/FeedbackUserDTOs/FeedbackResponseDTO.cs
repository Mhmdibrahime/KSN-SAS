namespace KSN_SAS.Models.DTOs.FeedbackUserDTOs
{
    public class FeedbackResponseDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string UserJob { get; set; }
    }
}