namespace Domain.DTOs
{
    public class RoleUserDto
    {
        public string RoleName { get; set; }

        public IList<UserInfoDto> UsersInRole { get; set; }
    }
}
