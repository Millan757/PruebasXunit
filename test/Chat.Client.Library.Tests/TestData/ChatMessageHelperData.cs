using System;
using Chat.Common.Entities;
using Xunit;

namespace Chat.Client.Library.Tests.TestData
{
    public class ChatMessageHelperData : TheoryData<ChatMessage, string>
    {
        public ChatMessageHelperData()
        {
            var date = DateTime.Now;
            Add(new ChatMessage { Author = "Author1", Date = date, Message = "Hello everybody!" }, $"{date:HH mm ss}:Author1->Hello everybody!");
            Add(new ChatMessage { Author = "Author2", Date = date.AddMinutes(1), Message = "Hello Author1" }, $"{date.AddMinutes(1):HH mm ss}:Author2->Hello Author1");
            Add(new ChatMessage { Author = "Author1", Date = date.AddMinutes(2), Message = "How are you?" }, $"{date.AddMinutes(2):HH mm ss}:Author1->How are you?");
        }
    }
}
