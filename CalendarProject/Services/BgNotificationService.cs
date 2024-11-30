using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CalendarProject.Contracts.Services;
using CalendarProject.Helpers;

namespace CalendarProject.Services
{
    internal class BgNotificationService
    {
        private Timer _timer;
        public bool isStarted { private get; set; }

        public BgNotificationService() 
        {
            _timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }

        private void Callback(object? state)
        {
            App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
        }
    }
}
