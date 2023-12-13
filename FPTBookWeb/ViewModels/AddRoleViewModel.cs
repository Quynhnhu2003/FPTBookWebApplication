using System.ComponentModel.DataAnnotations;

namespace FPTBookWeb.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        public string RoleName { get; set; }
    }
}
