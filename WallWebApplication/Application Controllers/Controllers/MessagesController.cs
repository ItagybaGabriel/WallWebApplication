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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService) 
        {
            this._messageService = messageService;
        }

        // GET
        [HttpGet("listar-minhasMensagens")]
        public async Task<ActionResult<IEnumerable<Message>>> GetOnlyMyMessages()
        {
            try
            {
                return Ok(await _messageService.GetOnlyMyMessages());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("listar-Mensagens-ChatId")]
        public async Task<ActionResult<Message>> GetMessagesByChatId([FromQuery] int chatId)
        {
            try
            {
                return Ok(await _messageService.GetMessagesByChatId(chatId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET
        [HttpGet("listar-Mensagens-Id")]
        public async Task<ActionResult<Message>> GetMessagesById([FromQuery] int messageId)
        {
            try
            {
                return Ok(await _messageService.GetMessageById(messageId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [HttpPost("alterar-minhaMensagem")]
        public async Task<IActionResult> UpdateMessage(Message Message)
        {
            try
            {
                return Ok(await _messageService.UpdateMessage(Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST
        [HttpPost("criar-novaMensagem")]
        public async Task<ActionResult<Message>> CreateNewMessage(Message Message, [FromQuery] int chatId, [FromQuery] string recipientUserId)
        {
            try
            {
                return Ok(await _messageService.CreateNewMessage(Message, chatId, recipientUserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE
        [HttpDelete("deletar-minhaMensagem")]
        public async Task<IActionResult> DeletePrivateMessage(int messageId)
        {
            try
            {
                return Ok(await _messageService.DeleteMessage(messageId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
