using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.FrontendModels.ArgsModels.CourseController;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.FrontendModels.ResponseModels.CourseController
{
    public class GetCoursesResponse
    {
        public GetCoursesResponse(List<FrontendCourse> courses, int statusCode, int totalPages)
        {
            Courses = courses;
            StatusCode = statusCode;
            TotalPages = totalPages;
        }

        public GetCoursesResponse(string description, int statusCode, int totalPages)
        {
            Description = description;
            StatusCode = statusCode;
            TotalPages = totalPages;
        }

        public List<FrontendCourse>? Courses { get; set; }
        public string? Description { get; set; }
        public int StatusCode { get; set; }
        public int TotalPages { get; set; }
    }
}