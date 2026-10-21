using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.AspNetCore.Mvc;
using KeelteKoolV2.Models.Lecturers;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2.Controllers
{
    public class LecturersController : Controller
    {
        private readonly KeelteKoolV2Context _context;
        private readonly ILecturersServices _lecturersServices;
        private readonly IFilesServices _filesServices;

        public LecturersController(
            KeelteKoolV2Context context,
            ILecturersServices lecturersServices,
            IFilesServices filesServices)
        {
            _context = context;
            _lecturersServices = lecturersServices;
            _filesServices = filesServices;
        }
        public async Task<IActionResult> Index()
        {
            var lecturers = await _context.Lecturers
                .Select(x => new LecturerViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Qualifications = x.Qualifications,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderBy(x => x.LastName)
                .Take(20)
                .ToListAsync();

            return View(lecturers);
        }
    }
}