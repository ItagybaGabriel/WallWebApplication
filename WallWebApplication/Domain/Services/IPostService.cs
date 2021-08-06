using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Domain.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<IEnumerable<Post>> GetOnlyMyPosts();
        Task<IEnumerable<Post>> GetPostsByUserId(string userId);
        Task<Post> GetPostById(int id);
        Task<Post> UpdatePost(Post post);
        Task<Post> LikePost(int postId);
        Task<Post> UnLikePost(int postId);
        Task<Post> CreateNewPost(Post post);
        Task<Post> DeletePost(int postId);

    }
}
