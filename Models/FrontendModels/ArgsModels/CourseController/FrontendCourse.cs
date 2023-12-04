using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.FrontendModels.ArgsModels.CourseController
{
    public class FrontendCourse
    {
        public FrontendCourse(string name, string authorId, int courseId, Category category)
        {
            Name = name;
            AuthorId = authorId;
            CourseId = courseId;
            Category = category;
        }

        public string Name { get; set; }
        public string AuthorId { get; set; }
        public int CourseId { get; set; }
        public Category Category { get; set; }
    }
}