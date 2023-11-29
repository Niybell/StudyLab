using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Models.FrontendModels.ResponseModels.Services
{
    public class CreateUserResponse
    {
        public CreateUserResponse(string description, int statusCode, User? createdUser)
        {
            Description = description;
            StatusCode = statusCode;
            CreatedUser = createdUser;
        }

        public string Description { get; set; }
        public int StatusCode { get; set; }
        public User? CreatedUser { get; set; }
    }
}