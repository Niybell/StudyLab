using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Models.FrontendModels.ResponseModels.AuthController
{
    public class GetAccountDateUser
    {
        public GetAccountDateUser(string name, Role role)
        {
            Name = name;
            Role = role;
        }

        public string Name { get; set; }
        public Role Role { get; set; }
    }
}