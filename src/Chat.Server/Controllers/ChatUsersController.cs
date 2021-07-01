using Chat.Common.Entities;
using Chat.Server.Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Chat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatUsersController : ControllerBase
    {
        private readonly ChatDbContext _context;

        public ChatUsersController(ChatDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ChatUser>> GetChatUser(string username, string password)
        {
            return await _context.ChatUsers.FirstOrDefaultAsync(user => user.Name == username && user.Password == password);
        }

        [HttpPost]
        public async Task<ActionResult<ChatUser>> PostChatUser(string username, string password)
        {
            var newUser = new ChatUser { Name = username, Password = password };
            _context.ChatUsers.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

    }
}
