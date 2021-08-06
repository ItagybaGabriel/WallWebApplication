using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Domain.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetOnlyMyMessages();
        Task<IEnumerable<Message>> GetMessagesByChatId(int chatId);
        Task<Message> GetMessageById(int id);
        Task<Message> UpdateMessage(Message message);
        Task<Message> CreateNewMessage(Message message, int chatId, string recipientUserId);
        Task<Message> DeleteMessage(int messageId);
  
    }
}
