using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization_roles_token.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>

    {
        public ApplicationUserStore(ApplicationDbContext context) : base(context)
        {

        }
    }
}
