using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Choice.Models
{
    public class Student : IdentityUser
    {
        public string Group { get; set; }

        public List<Discipline> Disciplines { set; get; }
    }
}
