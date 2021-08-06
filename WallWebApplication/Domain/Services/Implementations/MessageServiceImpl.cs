using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;
using WallWebApplication.Infra.Repositories;

namespace WallWebApplication.Domain.Services
{
    public class MessageServiceImpl : IMessageService
    {

        private readonly MessageRepository _messageRepository;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IChatService _chatService;

        public MessageServiceImpl(MessageRepository messageRepository, IAuthService authService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IChatService chatService)
        {
            this._messageRepository = messageRepository;
            this._authService = authService;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._chatService = chatService;
        }
        public async Task<IEnumerable<Message>> GetOnlyMyMessages()
        {
            var allMessageslist = await _messageRepository.GetAllMessagesAsync();
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            var onlyMyMessageslist = allMessageslist.Where(o => o.UserId == userId);
            if (onlyMyMessageslist == null)
                throw new ArgumentException($"Não foi possível encontrar mensagens do usuario com Id = '{userId}'");

            return onlyMyMessageslist;
        }
        public async Task<IEnumerable<Message>> GetMessagesByChatId(int chatId)
        {
            var allMessageslist = await _messageRepository.GetAllMessagesAsync();
            var currentUser = await _authService.GetCurrentUser();
            var messagesListByChatId = allMessageslist.Where(o => o.ChatId == chatId);
            if (messagesListByChatId.Count() == 0)
                throw new ArgumentException($"Não foi possível encontrar messages do chat com Id = '{chatId}'");
            var teste = messagesListByChatId.Where(o => o.UserId == currentUser.Id);
            if (teste.Count() == 0)
                throw new ArgumentException($" {currentUser.UserName} ainda não mandou nenhuma mensagem no chat com Id = '{chatId}'");

            return messagesListByChatId;
        }

        public async Task<Message> GetMessageById(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);

            if (message == null)
            {
                throw new ArgumentException($"Não foi possível achar o message com Id = '{id}'");
            }

            var User = await _authService.GetCurrentUser();
            if (message.UserId == User.Id)
                return message;

            throw new ArgumentException($"Você não tem permissão para ver a mensagem com Id = '{message.Id}'");
        }

        public async Task<Message> UpdateMessage(Message message)
        {
            Message findMessage = await _messageRepository.GetMessageByIdAsync(message.Id);
            if (findMessage == null)
                throw new ArgumentException($"Não foi possível aleterar pois não encontramos uma mensagem com Id = '{message.Id}'");

            var User = await _authService.GetCurrentUser();
            if (findMessage.UserId == User.Id)
            {
                findMessage.MessageBody = message.MessageBody;
                return await _messageRepository.UpdateMessageAsync(findMessage);
            }

            throw new ArgumentException($"Você não tem permissão para alterar esta mensagem com Id = '{message.Id}'");
        }

        public async Task<Message> CreateNewMessage(Message message, int chatId, string recipientUserId)
        {
            var currentUser = await _authService.GetCurrentUser();
            Chat checkChat= await _chatService.GetChatById(chatId);

            if (checkChat == null) {
                Chat novochat = new Chat();
                message.Chat = await _chatService.CreateNewChat(novochat);
                novochat.Messages.Add(message);

                novochat.SenderUser = currentUser;
                novochat.SenderUserID = currentUser.Id;
                var recipientUser = await _authService.GetUserById(recipientUserId);
                novochat.RecipientUser = recipientUser;
                novochat.RecipientUserID = recipientUserId;

                message.Data = DateTime.Now;
                message.User = currentUser;
                message.UserId = currentUser.Id;
                message.ChatId = novochat.ChatId;

                return await _messageRepository.CreateNewMessageAsync(message);
            }

            message.Chat = checkChat;
            message.Chat.Messages.Add(message);
            message.ChatId = checkChat.ChatId;
            message.Data = DateTime.Now;
            message.User = currentUser;
            message.UserId = currentUser.Id;

            return await _messageRepository.CreateNewMessageAsync(message);
        }

        public async Task<Message> DeleteMessage(int messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                throw new ArgumentException($"Não foi possível deletar pois não encontramos um message com Id = '{messageId}'");
            }

            var User = await _authService.GetCurrentUser();
            if (message.UserId == User.Id)
                return await _messageRepository.DeleteMessageAsync(messageId);

            throw new ArgumentException($"Você não tem permissão para deletar a mensagem com Id = '{message.Id}'");
        }

    }
}
