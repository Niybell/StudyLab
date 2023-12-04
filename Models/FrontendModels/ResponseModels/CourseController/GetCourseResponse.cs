using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.FrontendModels.ResponseModels.CourseController
{
    public class GetCourseResponse
    {
        public GetCourseResponse(string description, int statusCode)
        {
            Description = description;
            StatusCode = statusCode;
        }

        public GetCourseResponse(Course course, int statusCode)
        {
            Course = course;
            StatusCode = statusCode;
        }

        public Course? Course { get; set; }
        public string? Description { get; set; }
        public int StatusCode { get; set; } 
    }
}