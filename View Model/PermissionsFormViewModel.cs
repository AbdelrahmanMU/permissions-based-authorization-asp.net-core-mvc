namespace permissions.View_Model
{
    public class PermissionsFormViewModel
    {
        public PermissionsFormViewModel()
        {
            roleClaims = new List<CheckBoxViewModel>();
        }

        public string id { get; set; }
        public string roleName { get; set; }
        public List<CheckBoxViewModel> roleClaims { get; set; }
    }
}
