using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyLab.Models.EntityFramework.Contexts;
using StudyLab.Models.ServerModels.Courses;

namespace StudyLab.Models.EntityFramework.Repositories
{
    public class CourseRepository: IBaseRepository<Course>
    {
        private AuthDBContext _context;

        public CourseRepository(AuthDBContext context)
        {
            _context = context;
        }

        public async Task Create(Course entity)
        {
            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Course entity)
        {
            _context.Courses.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Course> GetAll()
        {
            return _context.Courses;
        }

        public async Task<Course> Update(Course entity)
        {
            _context.Courses.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}