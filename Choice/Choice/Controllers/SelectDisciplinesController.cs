using Choice.Data;
using Choice.Models;
using Choice.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Choice.Controllers
{

    [Authorize(Roles = "student")]
    public class SelectDisciplinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Student> _userManager;

        public SelectDisciplinesController(ApplicationDbContext context, UserManager<Student> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            Student currentStudent = await _context.Users
                .Include(s => s.Disciplines)
                .ThenInclude(d => d.Teacher)
                .SingleOrDefaultAsync(s => s.UserName == User.Identity.Name);
            return View(currentStudent);
        }

        [HttpGet]
        public IActionResult SelectDisciplines()
        {
            var student = _context.Users
                .Include(s => s.Disciplines)
                .SingleOrDefault(s => s.UserName == User.Identity.Name);
            var disciplines = _context.Disciplines.ToList();
            List<SelectedDisciplineViewModel> selectedDisciplines = new List<SelectedDisciplineViewModel>();
            foreach (var disc in disciplines)
            {
                selectedDisciplines.Add(new SelectedDisciplineViewModel
                { Discipline = disc, IsSelected = student.Disciplines.Contains(disc) });
            }
            StudentDisciplinesViewModel studentDisciplines = new StudentDisciplinesViewModel
            {
                Disciplines = selectedDisciplines,
                Student = student,
            };
            return View(studentDisciplines);
        }

        [HttpPost]
        public IActionResult SelectDisciplines(StudentDisciplinesViewModel studentDisciplines)
        {
            var student = _context.Users
                .Include(s => s.Disciplines)
                .SingleOrDefault(s => s.Id == studentDisciplines.Student.Id);
            student.Disciplines.Clear();
            foreach (var disc in studentDisciplines.Disciplines)
            {
                if (disc.IsSelected)
                {
                    var discipline = _context.Disciplines.SingleOrDefault(d => d.Id == disc.Discipline.Id);
                    student.Disciplines.Add(discipline);
                }
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
