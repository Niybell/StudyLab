using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.FrontendModels.ArgsModels.UsersController;
using StudyLab.Models.FrontendModels.ResponseModels.Base;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Utils.Validators.AuthController
{
    public class RegisterModelValidator
    {
        private RegisterModel _registerModel { get; set; }
        public RegisterModelValidator(RegisterModel registerModel)
        {
            _registerModel = registerModel;
        }
        public TextResponse IsValidate(IQueryable<User> users)
        {
            User? user = users.SingleOrDefault(u => u.UserName == _registerModel.Name);
            if (user != null) return new TextResponse("User with this name is already registered", 1);
            else if (_registerModel.Name.Length < 4) return new TextResponse("Minimum name length - 4 characters", 1);
            else if (!_registerModel.Email.Contains("@")) return new TextResponse("Email invalid", 1);
            else if (_registerModel.Password.Length < 8) return new TextResponse("Minimum password length - 8 characters", 1);
            else if (_registerModel.Password != _registerModel.PasswordConfirm) return new TextResponse("Passwords are different", 1);

            return new TextResponse("Validation passed", 0);
        }
    }
}