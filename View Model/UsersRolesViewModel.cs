using Microsoft.AspNetCore.Identity;
namespace permissions.View_Model
{
    public class UsersRolesViewModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public IEnumerable<string> roles { get; set; }

    }
}
