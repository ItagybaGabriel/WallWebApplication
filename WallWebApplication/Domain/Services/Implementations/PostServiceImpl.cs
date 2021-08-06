using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Data.Repositories;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Domain.Services
{
    public class PostServiceImpl : IPostService
    {
        private readonly PostRepository _postRepository;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostServiceImpl(PostRepository postRepository, IAuthService authService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._postRepository = postRepository;
            this._authService = authService;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var getAllPosts =  await _postRepository.GetAllPostsAsync();
            return getAllPosts;
        }

        public async Task<IEnumerable<Post>> GetOnlyMyPosts()
        {
            var allPostlist =  await _postRepository.GetAllPostsAsync();
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            
            var onlyMyPostlist = allPostlist.Where(o => o.UserId == userId);
            if(onlyMyPostlist == null)
                throw new ArgumentException($"Não foi possível encontrar posts do usuario com Id = '{userId}'");

            return onlyMyPostlist;
        }
        public async Task<IEnumerable<Post>> GetPostsByUserId(string userId)
        {
            var allPostlist = await _postRepository.GetAllPostsAsync();

            var Postlist = allPostlist.Where(o => o.UserId == userId);
            if (Postlist == null)
                throw new ArgumentException($"Não foi possível encontrar posts do usuario com Id = '{userId}'");

            return Postlist;
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                throw new ArgumentException($"Não foi possível achar o post com Id = '{id}'");
            }

            return post;
        }

        public async Task<Post> UpdatePost(Post post)
        {
            var currentUser = await _authService.GetCurrentUser();

            Post findPost = await _postRepository.GetPostByIdAsync(post.Id);
            if (findPost == null)
                throw new ArgumentException($"Não foi possível aleterar pois não encontramos um post com Id = '{post.Id}'");
            if (post.UserId == currentUser.Id)
            {
                findPost.Titulo = post.Titulo;
                findPost.Conteudo = post.Conteudo;

                return await _postRepository.UpdatePostAsync(findPost);
            }

            throw new ArgumentException($"Você não tem autorização para alterar um post que não foi publicado por você");
        }

        public async Task<Post> LikePost(int postId)
        {
            var findPost = await _postRepository.GetPostByIdAsync(postId);
            if (findPost == null)
                throw new ArgumentException($"Não foi possível dar 'Like' pois não encontramos um post com Id = '{postId}'");

            Like novoLike = new Like();
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            if (findPost.Likes.Count(o => o.UserId == currentUser.Id) > 0)
            {
                throw new ArgumentException("Opa! Você já deu 'Like' neste post!");
            }

            novoLike.Data = DateTime.Now;
            novoLike.UserId = currentUser.Id;
            findPost.LikesCount += 1;
            findPost.Likes.Add(novoLike);

            return await _postRepository.UpdatePostAsync(findPost);
        }

         public async Task<Post> UnLikePost(int postId)
        {
            var findPost = await _postRepository.GetPostByIdAsync(postId);
            if (findPost == null)
                throw new ArgumentException($"Não foi possível dar 'Like' pois não encontramos um post com Id = '{postId}'");

            ApplicationUser currentUser = await _authService.GetCurrentUser();

            if (findPost.Likes.Count(o => o.UserId == currentUser.Id) == 0)
            {
                throw new ArgumentException("Opa! Você não pode dar 'Unlike' pois primeiro precisa dar 'Like' neste post!");
            }
            findPost.LikesCount -= 1;
            var item = findPost.Likes.SingleOrDefault(x => x.UserId == currentUser.Id);
            if (item != null)
                findPost.Likes.Remove(item);

            return await _postRepository.UpdatePostAsync(findPost);
        }

        public async Task<Post> CreateNewPost(Post post)
        {
            post.Date = DateTime.Now;
            post.LikesCount = 0;
           
            post.Likes = new List<Like>();

            post.User = await _authService.GetCurrentUser();
            
            return await _postRepository.CreateNewPostAsync(post);
        }

        public async Task<Post> DeletePost(int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null)
            {
               throw new ArgumentException($"Não encontramos um post com Id = '{postId}'");
            }

            var User = await _authService.GetCurrentUser();
            if (post.UserId == User.Id)
                return await _postRepository.DeletePostAsync(postId);
            
            throw new ArgumentException($"Você não tem permissão para deletar o post com Id = '{post.Id}'");
        }
    }
}
