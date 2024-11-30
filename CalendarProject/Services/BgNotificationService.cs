using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CalendarProject.Contracts.Services;
using CalendarProject.Helpers;
using CalendarProject.EntityFramework;

namespace CalendarProject.Services
{
    internal class BgNotificationService
    {
        public bool isStarted { get; private set; }
        public Dictionary<DateTime, Event> notifDict { get; private set; }

        private List<Timer> _timers = new List<Timer>();
        private DbWorker dbWorker;

        public void Initialize()
        {
            dbWorker = App.GetService<DbWorker>();
            notifDict = new Dictionary<DateTime, Event>();
            InflateNotifDict();
            InflateTimers();
        }

        private void InflateNotifDict()
        {
            foreach (var item in dbWorker.DbExecuteSQL<Event>(
                "SELECT * FROM Events WHERE UserId = @p0",
                SessionContext.CurrentUser.Id))
            {
                if (item.NotifTime != null)
                {
                    notifDict.Add(item.NotifTime.Value, item);
                }
            };
        }

        private void InflateTimers()
        {
            foreach (var k in notifDict.Keys)
            {
                TimeSpan dueTime = notifDict[k].NotifTime!.Value - DateTime.Now;
                if (dueTime > TimeSpan.Zero)
                {
                    _timers.Add(new Timer(Callback, k, dueTime, Timeout.InfiniteTimeSpan));
                }                
            }
        }

        private void Callback(object? state)
        {
            if (isStarted && state is DateTime time)
            {
                var eventItem = notifDict[time];
                string notification = string.Format(
                    "<toast launch=\"action=ToastClick\">" +
                    "   <visual> " +
                    "       <binding template=\"ToastGeneric\">" +
                    "           <text>{0}</text>" +
                    "           <text>{1}</text>" +
                    "           <image placement=\"appLogoOverride\" hint-crop=\"circle\" src=\"{2}Assets/WindowIcon.ico\"/>" +
                    "       </binding>" +
                    "   </visual>" +
                    "   <actions>" +
                    "       <action content=\"View\" arguments=\"header={0};descr={1}\"/>" +
                    "   </actions>" +
                    "</toast>",
                    eventItem.Header,
                    eventItem.Description,
                    AppContext.BaseDirectory
                );

                App.GetService<IAppNotificationService>().Show(notification);
            }
        }

        public void Start() => isStarted = true;

        public void Stop() => isStarted = false;
    }
}
