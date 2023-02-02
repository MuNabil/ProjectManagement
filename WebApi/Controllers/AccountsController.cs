using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _unitOfWork.Accounts.Register(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _unitOfWork.Accounts.Login(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _unitOfWork.Accounts.Logout();
            return Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var model = new UserChangePasswordDto()
            {
                UserId = User.Claims.FirstOrDefault(o => o.Type == "uid")?.Value,
                NewPassword = dto.NewPassword,
                OldPassword = dto.OldPassword
            };
            if(await _unitOfWork.Accounts.ChangePassword(model))
                return Ok();

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _unitOfWork.Accounts.ResetPassword(dto);

            if (!result.IsAuthenticated) return NotFound(result.Message);

            return Ok(result.Message);
        }
    }
}
