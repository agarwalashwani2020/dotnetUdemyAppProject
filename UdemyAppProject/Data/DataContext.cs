using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAppProject.Entities;

namespace UdemyAppProject.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
                
        }


        public DbSet<AppUser> Users { get; set; }
    }
}
