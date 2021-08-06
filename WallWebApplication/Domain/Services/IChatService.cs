using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Domain.Services
{
    public interface IChatService
    {

        Task<IEnumerable<Chat>> GetOnlyMyChats();
        Task<Chat> GetChatById(int chatId);
        Task<Chat> CreateNewChat(Chat chat);
        Task<Chat> DeleteChat(int chatId);

    }
}
