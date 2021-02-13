using Choice.Models;
using System.Collections.Generic;

namespace Choice.ViewModels
{
    public class StudentDisciplinesViewModel
    {
        public Student Student { get; set; }
        public List<SelectedDisciplineViewModel> Disciplines { get; set; }
    }
}
