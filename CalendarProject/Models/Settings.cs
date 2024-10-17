using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ThemeId { get; set; }
        public int LangId { get; set; }

        public User User { get; set; }
    }
}