using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Membership
{
    public class RiodeUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }
    }
}
