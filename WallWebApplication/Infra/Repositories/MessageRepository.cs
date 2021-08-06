using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Infra.Repositories
{
    public class MessageRepository
    {
        private readonly MySQLContext _context;

        public MessageRepository(MySQLContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Message>> GetAllMessagesAsync()
        {
            return await _context.Message.ToListAsync();
        }
        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            var post = await _context.Message.Include(p => p.User).FirstOrDefaultAsync(p => p.Id.Equals(messageId));
            //await _context.Post.Include(p => p.Date).FirstOrDefaultAsync(p => p.Id.Equals(messageId));

            return await _context.Message.FindAsync(messageId);
        }

        public async Task<Message> UpdateMessageAsync(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.Entry(message).Entity;
        }

        public async Task<Message> CreateNewMessageAsync(Message message)
        {

            await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<Message> DeleteMessageAsync(int messageId)
        {
            var message = await _context.Message.FindAsync(messageId);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}
