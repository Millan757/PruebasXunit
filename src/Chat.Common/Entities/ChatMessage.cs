using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Entities
{
    public class ChatMessage
    {
        [Key]
        public int IdChatMessage { get; set; }

        [StringLength(15)]
        [Required]
        public string Author { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
