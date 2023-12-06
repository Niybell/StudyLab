using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.ServerModels.Courses.Lessons.Steps
{
    public class Step
    {
        public Step()
        {
        }

        public Step(StepType type, string htmlCode, Test? test)
        {
            Type = type;
            HtmlCode = htmlCode;
            Test = test;
        }

        public int Id { get; set; }
        public StepType? Type { get; set; }
        public string? HtmlCode { get; set; }
        public Test? Test { get; set; }
    }
}