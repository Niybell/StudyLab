using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyLab.Models.FrontendModels.ArgsModels.UsersController;
using StudyLab.Models.FrontendModels.ResponseModels.AuthController;
using StudyLab.Models.FrontendModels.ResponseModels.Base;
using StudyLab.Models.FrontendModels.ResponseModels.Services;
using StudyLab.Models.ServerModels.User;
using StudyLab.Services.Interfaces;
using StudyLab.Utils.Validators.AuthController;

namespace StudyLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private UserManager<User> _userManager { get; set; }
        private SignInManager<User> _singInManager;
        private IAuthService _authService { get; set; }

        public AuthController(UserManager<User> userManager, SignInManager<User> singInManager, IAuthService authService)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<TextResponse> Register(RegisterModel model)
        {
            RegisterModelValidator validator = new RegisterModelValidator(model);

            TextResponse validateResponse = validator.IsValidate(_userManager.Users);

            if (validateResponse.StatusCode == 1)
                return new TextResponse(validateResponse.Description, 500);

            CreateUserResponse createUserResponse = await _authService.CreateUserAsync(model, _userManager);

            if (createUserResponse.StatusCode == 200)
                await _singInManager.SignInAsync(createUserResponse.CreatedUser, false);
            else
                return new TextResponse(createUserResponse.Description, 500);

            return new TextResponse("Successfully register", 200);
        }
        [HttpGet("logout")]
        public async Task<TextResponse> Logout()
        {
            await _singInManager.SignOutAsync();
            return new TextResponse("Successfully logout", 200);
        }

        [HttpPost("login")]
        public async Task<TextResponse> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var loginResult = await _singInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

            if (loginResult.Succeeded)
                return new TextResponse("Successfully login", 200);
            else
                return new TextResponse("Login or password is incorrect", 500);
        }
        [HttpGet("get-account-date")]
        public async Task<GetAccountDateResponse> GetAccountDate()
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);

            GetAccountDateUser? getAccountDateUser = null;
            if (user != null)
            {
                getAccountDateUser = new GetAccountDateUser(
                    user.UserName, user.Role
                );
            }

            return new GetAccountDateResponse(getAccountDateUser, 200);
        }


    }
}