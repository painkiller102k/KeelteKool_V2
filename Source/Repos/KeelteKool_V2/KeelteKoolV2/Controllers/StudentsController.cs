using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Models.Students;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsServices _studentsServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public StudentsController(
            IStudentsServices studentsServices,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _studentsServices = studentsServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var student = await _studentsServices.DetailAsync(user.Id);

            if (student == null)
            {
                return NotFound();
            }

            var vm = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                UserID = student.UserID,
                CreatedAt = student.CreatedAt,
                ModifiedAt = student.ModifiedAt
            };

            return View(vm);
        }
    }
}