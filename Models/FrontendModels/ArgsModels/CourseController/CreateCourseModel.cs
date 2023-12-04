using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.FrontendModels.ArgsModels.CourseController
{
    public class CreateCourseModel
    {
        public CreateCourseModel(string name, string logoURL, Category category, string description, Language language, Difficulty difficulty, string load, string result, string aboutCourse, string targetAudience, string initialRequirements, string howTrainingWorks)
        {
            Name = name;
            LogoURL = logoURL;
            Category = category;
            Description = description;
            Language = language;
            Difficulty = difficulty;
            Load = load;
            Result = result;
            AboutCourse = aboutCourse;
            TargetAudience = targetAudience;
            InitialRequirements = initialRequirements;
            HowTrainingWorks = howTrainingWorks;
        }

        public string Name { get; set; }   
        public string LogoURL { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Load { get; set; }
        public string Result { get; set; }
        public string AboutCourse { get; set; }
        public string TargetAudience { get; set; }
        public string InitialRequirements { get; set; }
        public string HowTrainingWorks { get; set; }
    }
}