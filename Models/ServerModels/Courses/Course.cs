using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses.Modules;

namespace StudyLab.Models.ServerModels.Courses
{
    public class Course
    {
        public Course(string name)
        {
            Name = name;
        }
        public Course()
        {
        }

        public Course(string? name, string? logoURL, Category? courseCategory, string? description, Language? language, Difficulty? difficulty, string? load, string? result, string? aboutCourse, string? targetAudience, string? initialRequirements, string? howTrainingWorks, string? author)
        {
            Name = name;
            LogoURL = logoURL;
            CourseCategory = courseCategory;
            Description = description;
            Language = language;
            Difficulty = difficulty;
            Load = load;
            Result = result;
            AboutCourse = aboutCourse;
            TargetAudience = targetAudience;
            InitialRequirements = initialRequirements;
            HowTrainingWorks = howTrainingWorks;
            AuthorId = author;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AuthorId { get; set; }
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
        public List<CourseTeacher>? Teachers { get; set; }
        public List<Module> Modules { get; set; } = new List<Module>();
        public Status? Status { get; set; }
    }
}