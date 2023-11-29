using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyLab.Models.ServerModels.Courses;
using StudyLab.Models.ServerModels.User;

namespace StudyLab.Models.EntityFramework.Contexts
{
    public class AuthDBContext : IdentityDbContext<User>
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
    }
}