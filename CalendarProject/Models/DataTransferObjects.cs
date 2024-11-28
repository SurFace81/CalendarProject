using CalendarProject.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject.Models
{
    public class EventDto
    {
        public DateTime Date { get; set; }
    }

    public class EventEditDto
    {
        public Event eventForEdit { get; set; }
    }
}
