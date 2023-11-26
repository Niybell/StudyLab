using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses.Lessons;

namespace StudyLab.Models.ServerModels.Courses.Modules
{
    public class Module
    {
        public Module()
        {
        }

        public Module(string name, string description, List<Lesson> lessons)
        {
            Name = name;
            Description = description;
            Lessons = lessons;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Lesson>? Lessons { get; set; }
    }
}