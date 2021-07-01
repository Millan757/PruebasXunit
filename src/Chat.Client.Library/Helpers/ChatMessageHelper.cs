using Chat.Common.Entities;

namespace Chat.Client.Library.Helpers
{
    public static class ChatMessageHelper
    {
        public static string GenerateString(ChatMessage message)
        {
            return $"{message.Date:HH mm ss}:{message.Author}->{message.Message}";
        }
    }
}
