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
    public class ChatServiceImpl : IChatService
    {
        private readonly ChatRepository _chatRepository;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatServiceImpl(ChatRepository chatRepository, IAuthService authService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._chatRepository = chatRepository;
            this._authService = authService;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Chat>> GetOnlyMyChats()
        {
            var allChatlist = await _chatRepository.GetAllChatsAsync();
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            var onlyMyChatlist = allChatlist.Where(f => f.SenderUserID == userId || f.RecipientUserID== userId);
            return onlyMyChatlist;
        }

        public async Task<Chat> GetChatById(int chatId)
        {
            var currentUser = await _authService.GetCurrentUser();
            var chat = await _chatRepository.GetChatByIdAsync(chatId);

            //if(chat.RecipientUserID == currentUser.Id || chat.SenderUserID == currentUser.Id)
            return chat;

            throw new ArgumentException($"Você não participa do chat com Id = '{chatId}'");
        }

        public async Task<Chat> CreateNewChat(Chat chat)
    {
        chat.SenderUserID = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        chat.Messages = new List<Message>();

        return await _chatRepository.CreateNewChatAsync(chat);
    }

    public async Task<Chat> DeleteChat(int chatId)
    {
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat == null)
        {
            throw new ArgumentException($"Não foi possível deletar pois não encontramos um chat com Id = '{chatId}'");
        }

        var currentUser = await _authService.GetCurrentUser();
        if (chat.RecipientUserID == currentUser.Id || chat.SenderUserID == currentUser.Id)
            return await _chatRepository.DeleteChatAsync(chatId);
            
        throw new ArgumentException($"Você não tem autorização para deletar o chat com Id = '{chatId}'");
     }

    }
}
