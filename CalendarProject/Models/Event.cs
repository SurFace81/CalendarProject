using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject.Models
{
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
}