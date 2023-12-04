using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses;
using StudyLab.Models.ServerModels.Courses.Modules;

namespace StudyLab.Models.FrontendModels.ArgsModels.CourseController
{
    public class UpdateCourseModel
    {
        public int CourseId { get; set; }
        public string? LogoURL { get; set; }
        public Category? CourseCategory { get; set; }
        public string? Description { get; set; }
        public Language? Language { get; set; }
        public Difficulty? Difficulty { get; set; }
        public string? Load { get; set; }
        public string? Result { get; set; }
        public string? AboutCourse { get; set; }
        public string? TargetAudience { get; set; }
        public string? InitialRequirements { get; set; }
        public string? HowTrainingWorks { get; set; }
        public List<Module>? Modules { get; set; }
    }
}