
using authorization_roles_token.Identity;
using authorization_roles_token.ServiceContract;
using authorization_roles_token.Utility;
using authorization_roles_token.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace authorization_roles_token.Services
{
    public class UserServices:IUserService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly AppSetting _appSettings;
        public UserServices(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager, IOptions<AppSetting> appSettings)
        {
            _applicationSignInManager = applicationSignInManager;
            _applicationUserManager = applicationUserManager;
            _appSettings = appSettings.Value;
        }

        public async Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(loginViewModel.UserName);
                applicationUser.PasswordHash = "";

                //jwt token
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.role_Admin))
                    applicationUser.Role = SD.role_Admin;
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.role_Employee))
                    applicationUser.Role = SD.role_Employee;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);

                var tokenDiscriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,applicationUser.Id),
                        new Claim(ClaimTypes.Email,applicationUser.Email),
                        new Claim(ClaimTypes.Role,applicationUser.Role)

                    }),
                    Expires = DateTime.UtcNow.AddHours(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDiscriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);
                applicationUser.PasswordHash = "";

                return applicationUser;
            }
            else
            {
                return null;
            }
        }
    }
}
