using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudyLab.Models.FrontendModels.ArgsModels.UsersController;
using StudyLab.Models.FrontendModels.ResponseModels.Services;
using StudyLab.Models.ServerModels.User;
using StudyLab.Services.Interfaces;

namespace StudyLab.Services.Implementations
{
    public class AuthService : IAuthService
    {
        public async Task<CreateUserResponse> CreateUserAsync(RegisterModel model, UserManager<User> userManager)
        {
            User newUser = new User()
            {
                UserName = model.Name,
                Email = model.Email
            };

            var createUserResponse = await userManager.CreateAsync(newUser, model.Password);

            if (createUserResponse.Succeeded)
                return new CreateUserResponse("Successfully create user", 200, newUser);

            return new CreateUserResponse(createUserResponse.Errors.ToArray()[0].Description, 500, null);
        }
    }
}