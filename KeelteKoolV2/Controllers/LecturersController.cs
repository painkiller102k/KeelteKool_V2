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


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            LecturerCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new KeelteKoolV2.Core.DTO.LecturerDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Qualifications = model.Qualifications,
                UserID = model.UserID
            };

            var result = await _lecturersServices.Create(dto);

            if (result == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Lecturer could not be created.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync(id.Value);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerViewModel
            {
                Id = lecturer.Id,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                Qualifications = lecturer.Qualifications,
                CreatedAt = lecturer.CreatedAt,
                ModifiedAt = lecturer.ModifiedAt
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync(id.Value);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerCreateUpdateViewModel
            {
                Id = lecturer.Id,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                Qualifications = lecturer.Qualifications,
                UserID = lecturer.UserID,
                CreatedAt = lecturer.CreatedAt,
                ModifiedAt = lecturer.ModifiedAt
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            Guid id,
            LecturerCreateUpdateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new KeelteKoolV2.Core.DTO.LecturerDTO
            {
                Id = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Qualifications = model.Qualifications,
                UserID = model.UserID
            };

            var result = await _lecturersServices.Update(dto);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync(id.Value);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerViewModel
            {
                Id = lecturer.Id,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                Qualifications = lecturer.Qualifications,
                CreatedAt = lecturer.CreatedAt,
                ModifiedAt = lecturer.ModifiedAt
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _lecturersServices.Delete(id);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



    }
}