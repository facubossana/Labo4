using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Models
{
    public class ManageUserModel
    {
        public IdentityUser user { get; set; }
        public IdentityRole<string> role { get; set; }
    }
}
