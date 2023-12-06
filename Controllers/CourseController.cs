using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyLab.Models.EntityFramework.Repositories;
using StudyLab.Models.FrontendModels.ArgsModels.CourseController;
using StudyLab.Models.FrontendModels.ResponseModels.Base;
using StudyLab.Models.FrontendModels.ResponseModels.CourseController;
using StudyLab.Models.ServerModels.Courses;
using StudyLab.Models.ServerModels.Courses.Lessons;
using StudyLab.Models.ServerModels.Courses.Lessons.Steps;
using StudyLab.Models.ServerModels.Courses.Modules;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private UserManager<User> _userManager;
        private IBaseRepository<Course> _courseRepository;

        public CourseController(UserManager<User> userManager, IBaseRepository<Course> courseRepository)
        {
            _userManager = userManager;
            _courseRepository = courseRepository;
        }

        [HttpPost("create")]
        public async Task<TextResponse> CreateCourseAsync(CreateCourseModel model)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            if (_courseRepository.GetAll().SingleOrDefault(c => c.Name == model.Name) != null)
                return new TextResponse("Course with this name is already created", 500);

            Course course = new Course(
                model.Name, model.LogoURL, model.Category, model.Description,
                model.Language, model.Difficulty, model.Load, model.Result,
                model.AboutCourse, model.TargetAudience, model.InitialRequirements,
                model.HowTrainingWorks, user.Id
            );

            course.Modules = new List<Module>();
            course.Teachers = new List<CourseTeacher>();
            course.Status = Status.draft;

            await _courseRepository.Create(course);

            user.Courses.Add(new CourseId(_courseRepository.GetAll().Single(c => c.Name == course.Name).Id));

            await _userManager.UpdateAsync(user);

            return new TextResponse("Successfully create course", 200);
        }
        [HttpPut("update-status")]
        public async Task<TextResponse> UpdateStatusAsync(int courseId, Status status)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll().SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            course.Status = status;

            await _courseRepository.Update(course);

            return new TextResponse("Successfully update status.", 200);
        }
        [HttpPut("update")]
        public async Task<TextResponse> UpdateAsync(UpdateCourseModel model)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == model.CourseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            if (model.CourseCategory != null) course.CourseCategory = model.CourseCategory;
            if (model.AboutCourse != null) course.AboutCourse = model.AboutCourse;
            if (model.Description != null) course.Description = model.Description;
            if (model.Difficulty != null) course.Difficulty = model.Difficulty;
            if (model.HowTrainingWorks != null) course.HowTrainingWorks = model.HowTrainingWorks;
            if (model.InitialRequirements != null) course.InitialRequirements = model.InitialRequirements;
            if (model.Language != null) course.Language = model.Language;
            if (model.Load != null) course.Load = model.Load;
            if (model.LogoURL != null) course.LogoURL = model.LogoURL;
            if (model.Modules != null) course.Modules = model.Modules;
            if (model.Result != null) course.Result = model.Result;
            if (model.TargetAudience != null) course.TargetAudience = model.TargetAudience;

            await _courseRepository.Update(course);

            return new TextResponse("Successfully update", 200);
        }
        [HttpDelete("delete")]
        public async Task<TextResponse> DeleteAsync(string courseName, int courseId)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll().SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            if (courseName != course.Name) return new TextResponse("Course names are different", 500);

            await _courseRepository.Delete(course);

            return new TextResponse("Successfully delete course", 500);
        }
        [HttpGet("courses")]
        public async Task<GetCoursesResponse> GetCoursesAsync(Category? category, GetCoursesType type, int totalCount = 10, int page = 1)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new GetCoursesResponse("You must be authorized", 500, 0);

            List<Course> EfCourses = await _courseRepository.GetAll().Where(c => c.Status == Status.published).ToListAsync();

            if (category != null)
            {
                EfCourses = await _courseRepository.GetAll().Where(c => c.CourseCategory == category && c.Status == Status.published).ToListAsync();
            }

            if (type != GetCoursesType.Online)
            {
                EfCourses = await _courseRepository.GetAll().ToListAsync();
            }


            if (EfCourses.Count == 0)
            {
                return new GetCoursesResponse(new List<FrontendCourse>(), 200, 0);
            }

            int pagesCount = (int)Math.Ceiling((double)EfCourses.Count / (double)totalCount);

            if (page > pagesCount)
                return new GetCoursesResponse($"Max page is {pagesCount}", 500, 0);

            List<Course> courses = GetCoursesPaginate(totalCount, page, EfCourses);

            return new GetCoursesResponse(ConvertCoursesToFrontendCourses(courses), 200, pagesCount);
        }
        private List<Course> GetCoursesPaginate(int totalCount, int page, List<Course> courses)
        {
            int skipCount = totalCount * (page - 1);
            int elementsCount = totalCount;

            int elementsBeforeSkipCount = courses.Skip(skipCount).Count();

            if (elementsBeforeSkipCount < totalCount)
            {
                elementsCount = elementsBeforeSkipCount;
            }

            return courses.GetRange(skipCount, elementsCount);
        }
        private List<FrontendCourse> ConvertCoursesToFrontendCourses(List<Course> courses)
        {
            FrontendCourse[] frontendCourses = new FrontendCourse[courses.Count];

            for (int i = 0; i < courses.Count; i++)
            {
                Course course = courses[i];

                frontendCourses[i] = new FrontendCourse(
                    course.Name, course.AuthorId, course.Id, (Category)course.CourseCategory
                );
            }

            return frontendCourses.ToList();
        }
        [HttpGet("course")]
        public async Task<GetCourseResponse> GetCourse(int courseId)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new GetCourseResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return new GetCourseResponse("Course with this id don't found", 500);

            return new GetCourseResponse(course, 200);
        }
        [HttpPost("create-module")]
        public async Task<TextResponse> CreateModule(string name, int courseId, string description)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            if (course.Modules.SingleOrDefault(m => m.Name == name) != null)
                return new TextResponse("Module with this name is already created", 500);

            course.Modules.Add(new Module(name, description, new List<Lesson>()));

            await _courseRepository.Update(course);

            return new TextResponse("Successfully create module", 200);
        }
        [HttpPost("create-lesson")]
        public async Task<TextResponse> CreateLesson(string name, string moduleName, int courseId)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            Module? module = course.Modules.SingleOrDefault(m => m.Name == moduleName);

            if (module == null) return new TextResponse("Module with this name is not found", 500);

            if (module.Lessons.SingleOrDefault(l => l.Name == name) != null)
                return new TextResponse("Lesson with this name is already created", 500);

            Lesson lesson = new Lesson(name);

            course.Modules.Single(m => m.Name == module.Name).Lessons.Add(lesson);

            await _courseRepository.Update(course);

            return new TextResponse("Successfully create lesson", 200);
        }
        [HttpPost("create-step")]
        public async Task<TextResponse> CreateStep(int courseId, string moduleName, string lessonName, StepType type, string htmlCode)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            Module? module = course.Modules.SingleOrDefault(m => m.Name == moduleName);

            if (course.Modules.SingleOrDefault(m => m.Name == moduleName) == null)
                return new TextResponse("Module with this name is not found", 500);

            if (module.Lessons.SingleOrDefault(l => l.Name == lessonName) == null)
                return new TextResponse("Lesson with this id is not found", 500);

            course.Modules
                .SingleOrDefault(m => m.Name == moduleName).Lessons
                .SingleOrDefault(l => l.Name == lessonName)
                .Steps.Add(new Step(type, htmlCode, null));

            await _courseRepository.Update(course);

            return new TextResponse("Successfully create step", 200);
        }
        [HttpDelete("delete-module")]
        public async Task<TextResponse> DeleteModuleAsync(int courseId, string moduleName)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            Module? module = course.Modules.SingleOrDefault(m => m.Name == moduleName);

            if (course.Modules.SingleOrDefault(m => m.Name == moduleName) == null)
                return new TextResponse("Module with this name is not found", 500);

            course.Modules.Remove(course.Modules.Single(m => m.Name == moduleName));

            await _courseRepository.Update(course);

            return new TextResponse("Successfully delete module", 200);
        }
        [HttpDelete("delete-lesson")]
        public async Task<TextResponse> DeleteLessonAsync(int courseId, string moduleName, string lessonName)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            Module? module = course.Modules.SingleOrDefault(m => m.Name == moduleName);

            if (module == null)
                return new TextResponse("Module with this name is not found", 500);

            Lesson? lesson = module.Lessons.SingleOrDefault(l => l.Name == lessonName);

            if (lesson == null)
                return new TextResponse("Lesson with this id is not found", 500);

            course.Modules.Single(m => m.Name == moduleName).Lessons.Remove(lesson);

            await _courseRepository.Update(course);

            return new TextResponse("Successfully delete lesson", 200);
        }
        [HttpDelete("delete-step")]
        public async Task<TextResponse> DeleteStepAsync(int courseId, string moduleName, string lessonName, int stepNumber)
        {
            User? user = await AuthController.GetUserFromClaimsAsync(HttpContext, _userManager);

            if (user == null) return new TextResponse("You must be authorized", 500);

            Course? course = await _courseRepository.GetAll()
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Steps)
                .ThenInclude(s => s.Test)
                .SingleOrDefaultAsync(c => c.Id == courseId && c.AuthorId == user.Id);

            if (course == null) return new TextResponse("Course with this id don't found", 500);

            Module? module = course.Modules.SingleOrDefault(m => m.Name == moduleName);

            if (module == null)
                return new TextResponse("Module with this name is not found", 500);

            Lesson? lesson = module.Lessons.SingleOrDefault(l => l.Name == lessonName);

            if (lesson == null)
                return new TextResponse("Lesson with this id is not found", 500);

            Step? step = null;

            try
            {
                step = lesson.Steps[stepNumber + 1];
            }
            catch
            {
                step = null;
            }

            if (step == null)
                return new TextResponse("Step with this number is not found", 200);

            course.Modules
                .Single(m => m.Name == moduleName).Lessons
                .Single(l => l.Name == lessonName).Steps
                .Remove(step);
            
            await _courseRepository.Update(course);

            return new TextResponse("Successfully delete step", 200);
        }
    }
}