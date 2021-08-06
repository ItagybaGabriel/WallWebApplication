using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WallWebApplication;
using WallWebApplication.Domain.Models;
using WallWebApplication.Domain.Services;

namespace WallWebApplication.Controllers
{
    [Authorize]
    [Route("api/Posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            this._postService = postService;
        }

        // GET
        [HttpGet("listar-posts")]
        public async Task<ActionResult<Object>> GetAllPosts()
        {
            try
            {
                return Ok(await _postService.GetAllPosts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("listar-MeusPosts")]
        public async Task<ActionResult<Object>> GetOnlyMyPosts()
        {
            try
            {
                return Ok(await _postService.GetOnlyMyPosts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("listar-PostUser")]
        public async Task<ActionResult<Object>> GetPostsByUserId([FromQuery] string userId)
        {
            try
            {
                return Ok(await _postService.GetPostsByUserId(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            try
            {
                return Ok(await _postService.GetPostById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("alterar-post")]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            try
            {
                return Ok(await _postService.UpdatePost(post));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("like-post")]
        public async Task<IActionResult> PostLike(int postId)
        {
            try
            {
                return Ok(await _postService.LikePost(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("unlike-post")]
        public async Task<IActionResult> UnPostLike(int postId)
        {
            try
            {
                return Ok(await _postService.UnLikePost(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("novo-post")]
        public async Task<ActionResult<Post>> CreateNewPost(Post post)
        {
            try
            {
                return Ok(await _postService.CreateNewPost(post));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE
        [HttpDelete("deletar-post")]
        public async Task<IActionResult> DeletePost([FromQuery] int id)
        {
            try
            {
                return Ok(await _postService.DeletePost(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
