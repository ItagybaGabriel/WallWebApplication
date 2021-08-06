using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Infra.Repositories
{
    public class ChatRepository
    {
        private readonly MySQLContext _context;

        public ChatRepository(MySQLContext context)
        {
            this._context = context;
        }


        public async Task<ICollection<Chat>> GetAllChatsAsync()
        {
            return await _context.Chat.ToListAsync();
        }
        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            var chat = await _context.Chat.Include(p => p.Messages).FirstOrDefaultAsync(p => p.ChatId.Equals(chatId));
            await _context.Chat.Include(p => p.SenderUser).FirstOrDefaultAsync(p => p.ChatId.Equals(chatId));
            await _context.Chat.Include(p => p.RecipientUser).FirstOrDefaultAsync(p => p.ChatId.Equals(chatId));


            return chat;
        }

        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            _context.Entry(chat).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.Entry(chat).Entity;
        }

        public async Task<Chat> CreateNewChatAsync(Chat chat)
        {

            await _context.Chat.AddAsync(chat);
            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> DeleteChatAsync(int chatId)
        {
            var chat = await _context.Chat.FindAsync(chatId);
            _context.Chat.Remove(chat);
            await _context.SaveChangesAsync();

            return chat;
        }
    }
}
