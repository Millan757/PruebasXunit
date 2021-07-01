using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Entities
{
    public class ChatUser
    {
        [Key]
        public int IdUser { get; set; }

        [StringLength(15)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
