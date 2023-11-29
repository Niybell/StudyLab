using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.FrontendModels.ArgsModels.UsersController
{
    public class RegisterModel
    {
        public RegisterModel(string name, string email, string password, string passwordConfirm)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}