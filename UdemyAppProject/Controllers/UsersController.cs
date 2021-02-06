using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAppProject.Data;
using UdemyAppProject.Entities;

namespace UdemyAppProject.Controllers
{
    public class UsersController : BaseApiController
    {
        public DataContext _context { get; set; }
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //api/users/3
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsersById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
