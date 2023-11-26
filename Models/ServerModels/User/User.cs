using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.ServerModels.User
{
    public class User: IdentityUser
    {
        public string? AvatarURL { get; set; }
        public Role Role { get; set; } = Role.User;
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Course> WantToTake { get; set; } = new List<Course>();
        public List<Course> InLearning { get; set; } = new List<Course>();
    }
}