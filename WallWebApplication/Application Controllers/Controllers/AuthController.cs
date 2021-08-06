using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;
using WallWebApplication.Domain.Models.DTOs;
using WallWebApplication.Domain.Services;

namespace WallWebApplication.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IConfiguration configuration;

        public AuthController(IAuthService service, IConfiguration configuration)
        {
            this._service = service;
            this.configuration = configuration;
        }


        // GET
        [HttpGet("lista-usuarios")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            try
            {
                return Ok(await _service.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO signInDto)
        {
            SsoDTO ssoDto = await _service.SignIn(signInDto);

            if (ssoDto == null)
                return Unauthorized();

            return Ok(ssoDto);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO signUpDto)
        {
            try
            {
                SsoDTO ssoDto = await _service.SignUp(signUpDto);

                return Ok(ssoDto);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("current-user")]
        [Authorize]
        public async Task<IActionResult> getCurrentUser()
        {
            //return Ok(String.Format("Autenticado - {0}", User.Identity.Name));
            try
            {
                return Ok(await _service.GetCurrentUser());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }

}
