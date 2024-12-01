using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CalendarProject.Contracts.Services;
using CalendarProject.EntityFramework;

namespace CalendarProject.Services
{
    internal class BgNotificationService
    {
        public bool isStarted { get; private set; }
        private Dictionary<DateTime, Event> notifsDict = new Dictionary<DateTime, Event>();
        private Dictionary<int, Timer> timersDict = new Dictionary<int, Timer>();
        private DbWorker dbWorker;

        public void Initialize()
        {
            dbWorker = App.GetService<DbWorker>();
            dbWorker.DbDeleted += DbWorker_DbDeleted;
            dbWorker.DbAdded += DbWorker_DbAdded;

            InflateNotifs();
            InflateTimers();
        }

        private void InflateNotifs()
        {
            foreach (var item in dbWorker.DbExecuteSQL<Event>(
                "SELECT * FROM Events WHERE UserId = @p0",
                SessionContext.CurrentUser.Id)
            )
            {
                AddNotif(item);
            };
        }

        private void InflateTimers()
        {
            foreach (var k in notifsDict.Keys)
            {
                AddTimer(notifsDict[k]);
            }
        }

        private void AddNotif(Event item)
        {
            if (item.NotifTime != null)
            {
                notifsDict.Add(item.NotifTime.Value, item);
            }
        }

        private void AddTimer(Event item)
        {
            if (item.NotifTime != null)
            {
                TimeSpan dueTime = (DateTime)item.NotifTime - DateTime.Now;
                if (dueTime > TimeSpan.Zero)
                {
                    timersDict.Add(item.Id, new Timer(Callback, item, dueTime, Timeout.InfiniteTimeSpan));
                }
            }
        }

        private void DeleteNotif(Event item)
        {
            if (item.NotifTime != null && notifsDict.ContainsKey(item.NotifTime.Value))
            {
                notifsDict.Remove(item.NotifTime.Value);
            }
        }

        private void DeleteTimer(Event item)
        {
            if (timersDict.ContainsKey(item.Id))
            {
                timersDict.Remove(item.Id);
            }
        }

        private void Callback(object? state)
        {
            if (isStarted && state is Event eventItem)
            {
                string notification = string.Format(
                    "<toast launch=\"action=ToastClick\">" +
                    "   <visual> " +
                    "       <binding template=\"ToastGeneric\">" +
                    "           <text>{0}</text>" +
                    "           <text>{1}</text>" +
                    "           <image placement=\"appLogoOverride\" hint-crop=\"circle\" src=\"{3}Assets/WindowIcon.ico\"/>" +
                    "       </binding>" +
                    "   </visual>" +
                    "   <audio src=\"ms-winsoundevent:Notification.Reminder\"/>\"" +
                    "   <actions>" +
                    "       <action content=\"View\" arguments=\"header={0};descr={1};time={2}\"/>" +
                    "   </actions>" +
                    "</toast>",
                    eventItem.Header,
                    eventItem.Description,
                    eventItem.Time.ToString(),
                    AppContext.BaseDirectory
                );

                App.GetService<IAppNotificationService>().Show(notification);
            }
        }

        public void Start() => isStarted = true;

        public void Stop() => isStarted = false;

        private void DbWorker_DbAdded(object? sender, object e)
        {
            if (e is Event[] events)
            {
                foreach (var @event in events) if (@event.NotifTime != null)
                {
                    AddNotif(@event);
                    AddTimer(@event);
                }
            }
        }

        private void DbWorker_DbDeleted(object? sender, object e)
        {
            if (e is Event @event)
            {
                DeleteNotif(@event);
                DeleteTimer(@event);
            }
        }
    }
}
