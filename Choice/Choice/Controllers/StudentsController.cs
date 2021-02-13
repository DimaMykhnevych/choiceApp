using Choice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Choice.Controllers
{
    [Authorize(Roles ="admin")]
    public class StudentsController : Controller
    {
        UserManager<Student> _userManager;

        public StudentsController(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _userManager.FindByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                var res = await _userManager.CreateAsync(student, "Aa_1234");
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, "student");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("UserName", "Student was not created");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var student = await _userManager.FindByIdAsync(id);
            return View(student);
        }

        [HttpPost("Students/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            var student = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(student);
            return RedirectToAction(nameof(Index));
        }
    }
}
