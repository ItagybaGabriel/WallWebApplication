using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;
using WallWebApplication.Domain.Models.DTOs;

namespace WallWebApplication.Domain.Services
{
    public interface IAuthService
    {

        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<ApplicationUser> UpdateUser(ApplicationUser user);
        Task<Boolean> DeletarUsuario(string userId);
        Task<SsoDTO> SignIn(LoginDTO signInDto);
        Task<SsoDTO> SignUp(RegisterDTO signUpDto);
        Task<ApplicationUser> GetCurrentUser();

    }
}
