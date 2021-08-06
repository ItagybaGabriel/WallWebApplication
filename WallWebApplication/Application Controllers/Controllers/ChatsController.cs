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

namespace WallWebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            this._chatService = chatService;
        }

        // GET
        [HttpGet("listar-meusChats")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetOnlyMyChats()
        {
            try
            {
                return Ok(await _chatService.GetOnlyMyChats());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("listar-ChatById")]
        public async Task<ActionResult<Chat>> GetChatById([FromQuery] int chatId)
        {
            try
            {
                return Ok(await _chatService.GetChatById(chatId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        // DELETE
        [HttpDelete("deletar-meuChat")]
        public async Task<IActionResult> DeletePrivateMessage(int chatId)
        {
            try
            {
                return Ok(await _chatService.DeleteChat(chatId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
