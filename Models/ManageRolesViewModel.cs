using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Models
{
    public class ManageRolesViewModel
    {
        public string UserId { get; set; }
        public List<string> UserRoles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
