using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.ServerModels.User
{
    public class CourseId
    {
        public CourseId()
        {
        }

        public CourseId(int thisCourseId)
        {
            ThisCourseId = thisCourseId;
        }

        public int Id { get; set; }
        public int ThisCourseId { get; set; }
    }
}