using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject.EntityFramework
{
    public class User
    {
        public int Id { get; set; }
        public bool AutoLogin { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Logo { get; set; }

        public List<Event>? Events { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
        public DateTime NotifTime { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }

        public User User { get; set; }
    }

    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ThemeId { get; set; }
        public int LangId { get; set; }

        public User User { get; set; }
    }
}
