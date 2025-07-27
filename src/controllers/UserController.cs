using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leapcert_back.src.context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userRepository;

        public UserController(
            IUserService userRepository
            )
        {
            _userRepository = userRepository;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthUser([FromBody] LoginUserDto user)
        {
            var result = await _userRepository.Authenticate(user, HttpContext);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [HttpGet("validateToken")]
        public IActionResult ValidateToken([FromQuery] string token)
        {
            var result = _userRepository.ValidateToken(token);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return Ok(new { message = "disconnect" });
        }

        [Authorize]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userRepository.GetAllAsync();

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto usuario)
        {
            var result = await _userRepository.PostAsync(usuario);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}