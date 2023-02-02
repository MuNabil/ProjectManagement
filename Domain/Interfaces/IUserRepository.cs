using Domain.DTOs;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<string> AddUserToRole(AddUserToRoleDto model);
        Task<string> AddNewRole(AddNewRoleDto model);
        Task<List<UserInfoDto>> GetAllUsers();
        Task<List<RoleUserDto>> GetAllRoles();
        Task<string> RemoveUser(string userId);
        Task<string> RemoveRole(string roleName);
        Task<string> RemoveUserFromRole(AddUserToRoleDto model);
    }
}
