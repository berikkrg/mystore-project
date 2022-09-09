using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Please, Input a Role")]
        public List<IdentityRole> AllRoles { get; set; }
        [Required(ErrorMessage ="Please, Input a Role")]
        public IList<string> UserRoles { get; set; }

        public ChangeRoleViewModel ()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
