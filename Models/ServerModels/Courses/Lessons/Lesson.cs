using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses.Lessons.Steps;

namespace StudyLab.Models.ServerModels.Courses.Lessons
{
    public class Lesson
    {
        public Lesson()
        {
        }

        public Lesson(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Step> Steps { get; set; } = new List<Step>();
    }
}