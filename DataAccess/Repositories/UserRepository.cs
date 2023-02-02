using AutoMapper;
using DataAccess_EF.Data;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<string> AddUserToRole(AddUserToRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);


            return result.Succeeded ? string.Empty : "Someting went wrong";
        }

        public async Task<string> AddNewRole(AddNewRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);

            if (role is not null)
                return "This role is already exists!";

            var result = await _roleManager.CreateAsync(new IdentityRole( model.RoleName.Trim() ));

            return result.Succeeded ? string.Empty : "Someting went wrong";

        }

        public async Task<List<UserInfoDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserInfoDto> usersDto = new List<UserInfoDto>();

            foreach (var user in users)
                usersDto.Add(await MapAppUserToUserDto(user));

            return usersDto;
        }

        //public async Task<List<UserInfoDto>> GetAllUsers()
        //{
        //    return await _userManager.Users.Select( user => new UserInfoDto
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email,
        //        UserName = user.UserName,
        //        Roles = _userManager.GetRolesAsync(user).Result
        //    }).ToListAsync();
        //}

        public async Task<List<RoleUserDto>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(role => new RoleUserDto
            {
                RoleName = role.Name,
            }).ToListAsync();

            foreach (var role in roles)
            {
                IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync(role.RoleName);

                role.UsersInRole = new List<UserInfoDto>();

                foreach (var user in users)
                    role.UsersInRole.Add(await MapAppUserToUserDto(user)); 
            }

            return roles;
        }

        private async Task<UserInfoDto> MapAppUserToUserDto(ApplicationUser user)
        {
            var userDto = _mapper.Map<UserInfoDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);
            return userDto;
        }

        //public async Task<List<RoleUserDto>> GetAllRoles()
        //{
        //    return await _roleManager.Roles.Select(role => new RoleUserDto
        //    {
        //        RoleName = role.Name,
        //        UsersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result
        //    }).ToListAsync();
        //}

        public async Task<string> RemoveUser(string userId)
        {
            var user =await _userManager.FindByIdAsync(userId);

            if (user is null)
                return "Invalid User ID";

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? string.Empty : "Someting went wrong";
        }

        public async Task<string> RemoveRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
                return "Role is not exists!";

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded ? string.Empty : "Someting went wrong";
        }

        public async Task<string> RemoveUserFromRole(AddUserToRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || role is null)
                return "Invalid User or Role";

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            return result.Succeeded ? string.Empty : "Someting went wrong";
        }

    }
}
