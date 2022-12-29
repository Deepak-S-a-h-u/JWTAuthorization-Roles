using authorization_roles_token.Identity;
using authorization_roles_token.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization_roles_token.ServiceContract
{
    public interface IUserService
    {
        Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
    }
}
