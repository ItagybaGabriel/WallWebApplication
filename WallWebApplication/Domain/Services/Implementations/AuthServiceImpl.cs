﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;
using WallWebApplication.Domain.Models.DTOs;
using WallWebApplication.Infra.Repositories;

namespace WallWebApplication.Domain.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly UserRepository _userRepository;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthServiceImpl(UserRepository userRepository, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._userRepository = userRepository;

            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            List<ApplicationUser> listUsers = await _userRepository.GetAllUsersAsync();

            return listUsers;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            ApplicationUser user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                throw new ArgumentException(" Usuário não existe!");

            return user;
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
        {
            ApplicationUser findUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");

            return await _userRepository.UpdateUserAsync(user);

        }

        public async Task<Boolean> DeletarUsuario(string userId)
        {
            ApplicationUser findUser = await _userRepository.GetUserByIdAsync(userId);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");

            await _userRepository.DeleteUserAsync(userId);

            return true;
        }

        public async Task<SsoDTO> SignIn(LoginDTO signInDto)
        {
            var user = await _userManager.FindByNameAsync(signInDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, signInDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new SsoDTO(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
            }

            return null;
        }

        public async Task<SsoDTO> SignUp(RegisterDTO signUpDto)
        {
            var userExists = await _userManager.FindByNameAsync(signUpDto.Username);

            if (userExists != null)
                throw new ArgumentException("User already exists!");

            ApplicationUser user = new ApplicationUser()
            {
                Email = signUpDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signUpDto.Username
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
                throw new ArgumentException("User creation failed! Please check user details and try again.");

            // Faz Login automatico
            //var userExists = await _userManager.FindByNameAsync(registerDto.Username);

            return new SsoDTO("Logado", DateTime.Now);
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User); // Get user id:

            ApplicationUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            return user;
        }



    }
}
