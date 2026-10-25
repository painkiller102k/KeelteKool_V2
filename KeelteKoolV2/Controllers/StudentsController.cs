using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using KeelteKoolV2.Models.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2.Controllers
{
    [Authorize(Roles = "Student,Lecturer,Admin")]
    public class StudentsController : Controller
    {
        private readonly KeelteKoolV2Context _context;
        private readonly IStudentsServices _studentsServices;

        public StudentsController(KeelteKoolV2Context context, IStudentsServices studentsServices)
        {
            _context = context;
            _studentsServices = studentsServices;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderBy(x => x.LastName)
                .Take(20)
                .Select(x => new StudentViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentsServices.DetailAsync(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                CreatedAt = student.CreatedAt,
                ModifiedAt = student.ModifiedAt
            });
        }
    }
}
