using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Choice.Models
{
    public class Teacher
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { set; get; }

        public List<Discipline> Disciplines { set; get; }
    }
}
