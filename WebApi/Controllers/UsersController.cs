using Domain.Constants;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _unitOfWork.Users.GetAllUsers());
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _unitOfWork.Users.GetAllRoles());
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(GetUserClaims());
        }

        [HttpPost("addUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _unitOfWork.Users.AddUserToRole(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("AddNewRole")]
        public async Task<IActionResult> AddNewRole([FromBody] AddNewRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _unitOfWork.Users.AddNewRole(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid user ID");

            var result = await _unitOfWork.Users.RemoveUser(userId);

            return !string.IsNullOrEmpty(result) ? BadRequest(result) : Ok(userId);
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest("Invalid role name!");

            var result = await _unitOfWork.Users.RemoveRole(roleName);

            return !string.IsNullOrEmpty(result) ? BadRequest(result) : Ok(roleName);
        }

        [HttpDelete("DeleteUserFromRole")]
        public async Task<IActionResult> DeleteUserFromRole([FromBody] AddUserToRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _unitOfWork.Users.RemoveUserFromRole(model);

            return !string.IsNullOrEmpty(result) ? BadRequest(result) : Ok(model);
        }

        private AuthenticationDto? GetUserClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is null)
                return null;

            var userClaims = identity.Claims;

            return new AuthenticationDto
            {
                Message = "Claims Will Update when you Login again ",
                UserId = userClaims.FirstOrDefault(o => o.Type == "uid")?.Value,
                UserName = userClaims.FirstOrDefault(o => o.Type.ToLower().Contains("claims/nameidentifier"))?.Value,
                Email = userClaims.FirstOrDefault(o => o.Type.ToLower().Contains("claims/emailaddress"))?.Value,
                Roles = userClaims.Where(o => o.Type.ToLower().Contains("claims/role")).Select(o => o.Value)?.ToList(),
                IsAuthenticated = true
            };
        }
    }
}
