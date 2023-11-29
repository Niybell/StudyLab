using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.ServerModels.Courses
{
    public class CourseTeacher
    {
        public CourseTeacher()
        {
        }

        public CourseTeacher(int? userId, string? name, string? avatarURL)
        {
            TeacherId = userId;
            Name = name;
            AvatarURL = avatarURL;
        }

        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public string? Name { get; set; }
        public string? AvatarURL { get; set; }   
    }
}