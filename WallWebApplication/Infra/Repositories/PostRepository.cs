using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Data.Repositories
{
    public class PostRepository
    {
        private readonly MySQLContext _context;

        public PostRepository(MySQLContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Post.ToListAsync();
        }
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await _context.Post.Include(p => p.User).FirstOrDefaultAsync(p => p.Id.Equals(postId));
            await _context.Post.Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id.Equals(postId));

            return post;
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.Entry(post).Entity;
        }

        public async Task<Post> CreateNewPostAsync(Post post)
        {
            
            await _context.Post.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post> DeletePostAsync(int postId)
        {
            var post = await _context.Post.FindAsync(postId);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
           
            return post;
        }
    }
}
