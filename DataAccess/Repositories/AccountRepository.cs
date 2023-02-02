using AutoMapper;
using DataAccess_EF.Data;
using DataAccess_EF.Services;
using Domain.Constants;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace DataAccess_EF.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, ITokenService tokenService,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationDto> Register(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthenticationDto { Message = "This email is already exists!" };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthenticationDto { Message = "This Username is already exists!" };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
            }
            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            var jwtSecurityToken = await _tokenService.CreateJwtToken(user);

            return new AuthenticationDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { Roles.User.ToString() },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthenticationDto> Login(LoginDto model)
        {
            var authenticationModel = new AuthenticationDto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.Message = "Email or Password is incorrect!";
                return authenticationModel;
            }

            var jwtSecurityToken = await _tokenService.CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authenticationModel.IsAuthenticated = true;
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            authenticationModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authenticationModel.Roles = rolesList.ToList();

            return authenticationModel;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ChangePassword(UserChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) return false;

            //if (!await _userManager.CheckPasswordAsync(user, model.OldPassword)) return false;

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded) return true;
            return false;
        }

        public async Task<AuthenticationDto> ResetPassword(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) 
                return new AuthenticationDto { Message = $"There is no user with email {model.Email}" };

            await _userManager.RemovePasswordAsync(user);

            var result = await _userManager.AddPasswordAsync(user, model.Password);

            if (result.Succeeded)
                return new AuthenticationDto { 
                    Message = $"The password reseted to '{model.Password}' successfuly",
                    IsAuthenticated = true
                };

            return new AuthenticationDto { Message = "Fail to reset the password" };
        }
    }
}
