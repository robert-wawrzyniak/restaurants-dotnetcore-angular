namespace Restaurants.Dtos
{
    public class UserPermissionsDto : UserDto
    {
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
    }
}
