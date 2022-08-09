namespace permissions.View_Model
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            roles = new List<CheckBoxViewModel>();
        }
       
        public string id { get; set; }
        public string userName { get; set; }
        public List<CheckBoxViewModel> roles { get; set; }
    }
}
