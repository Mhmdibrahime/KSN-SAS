using System.ComponentModel.DataAnnotations;

namespace KSN_SAS.Models.Entities
{
    public class Message
    {
        [Key]
      public  int Id { get; set; }
        [MaxLength(10000)]
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }


    }
}
