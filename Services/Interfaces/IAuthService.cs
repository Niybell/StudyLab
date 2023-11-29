using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudyLab.Models.FrontendModels.ArgsModels.UsersController;
using StudyLab.Models.FrontendModels.ResponseModels.Services;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Services.Interfaces
{
    public interface IAuthService
    {
        Task<CreateUserResponse> CreateUserAsync(RegisterModel model, UserManager<User> userManager);

    }
}