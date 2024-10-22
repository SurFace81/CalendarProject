using CalendarProject.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject
{
    internal class SessionContext
    {
        public static User CurrentUser { get; set; }
        public static Settings CurrentSettings { get; set; }
    }
}
