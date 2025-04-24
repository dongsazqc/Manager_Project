namespace Project_Manager.Models
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // Danh sách các quyền người dùng đã có
        public IList<string> SelectedRoles { get; set; }

        // Danh sách các quyền có sẵn
        public IList<string> AvailableRoles { get; set; }
    }
}
