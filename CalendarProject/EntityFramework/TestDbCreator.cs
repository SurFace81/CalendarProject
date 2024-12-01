using System.Security.Cryptography;
using System.Text;

namespace CalendarProject.EntityFramework
{
#if DEBUG
    public class TestDbCreator
    {
        public User User1 { get; private set; }
        public User User2 { get; private set; }
        public User User3 { get; private set; }

        public Event Event1 { get; private set; }
        public Event Event2 { get; private set; }
        public Event Event3 { get; private set; }
        public Event Event4 { get; private set; }
        public Event Event5 { get; private set; }
        public Event Event6 { get; private set; }
        public Event Event7 { get; private set; }
        public Event Event8 { get; private set; }
        public Event Event9 { get; private set; }
        public Event Event10 { get; private set; }

        public Settings Settings1 { get; private set; }
        public Settings Settings2 { get; private set; }
        public Settings Settings3 { get; private set; }

        public TestDbCreator()
        {
            // Создание пользователей
            User1 = new User
            {
                Name = "Alice",
                Email = "alice@example.com",
                Password = GetMD5Hash("password123")
            };

            User2 = new User
            {
                Name = "Bob",
                Email = "bob@example.com",
                Password = GetMD5Hash("123")
            };

            User3 = new User
            {
                Name = "Charlie",
                Email = "charlie@example.com",
                Password = GetMD5Hash("abcdef")
            };

            // Создание событий
            Event1 = new Event
            {
                UserId = 1,
                Time = new DateTime(2024, 10, 5, 10, 0, 0),
                Date = new DateTime(2024, 10, 14),
                NotifTime = new DateTime(2024, 11, 30, 22, 14, 0),
                Header = "Team Meeting",
                Description = "Discuss project updates.",
                Priority = 1
            };
            Event2 = new Event
            {
                UserId = 1,
                Time = new DateTime(2024, 10, 10, 14, 0, 0),
                Date = new DateTime(2024, 10, 14),
                NotifTime = new DateTime(2024, 11, 30, 15, 53, 30),
                Header = "Client Presentation",
                Description = "Present to the client.",
                Priority = 2
            };
            Event3 = new Event
            {
                UserId = 2,
                Time = new DateTime(2024, 10, 12, 9, 0, 0),
                Date = new DateTime(2024, 10, 16),
                NotifTime = new DateTime(2024, 10, 11, 9, 0, 0),
                Header = "Doctor's Appointment",
                Description = "Annual check-up.",
                Priority = 1
            };
            Event4 = new Event
            {
                UserId = 2,
                Time = new DateTime(2024, 10, 15, 18, 0, 0),
                Date = new DateTime(2024, 10, 16),
                NotifTime = new DateTime(2024, 10, 14, 18, 0, 0),
                Header = "Dinner with Friends",
                Description = "Catch up with friends at the new restaurant.",
                Priority = 3
            };
            Event5 = new Event
            {
                UserId = 3,
                Time = new DateTime(2024, 10, 20, 11, 0, 0),
                Date = new DateTime(2024, 10, 16),
                NotifTime = new DateTime(2024, 10, 19, 11, 0, 0),
                Header = "Yoga Class",
                Description = "Weekly yoga class.",
                Priority = 2
            };
            Event6 = new Event
            {
                UserId = 1,
                Time = new DateTime(2024, 10, 22, 15, 30, 0),
                Date = new DateTime(2024, 10, 22),
                NotifTime = new DateTime(2024, 10, 21, 15, 30, 0),
                Header = "Workshop",
                Description = "Participate in the workshop on productivity.",
                Priority = 2
            };
            Event7 = new Event
            {
                UserId = 3,
                Time = new DateTime(2024, 10, 25, 19, 0, 0),
                Date = new DateTime(2024, 10, 25),
                NotifTime = new DateTime(2024, 10, 24, 19, 0, 0),
                Header = "Concert",
                Description = "Attend the concert of your favorite band.",
                Priority = 3
            };
            Event8 = new Event
            {
                UserId = 2,
                Time = new DateTime(2024, 10, 30, 20, 0, 0),
                Date = new DateTime(2024, 10, 30),
                NotifTime = new DateTime(2024, 10, 29, 20, 0, 0),
                Header = "Movie Night",
                Description = "Watch a movie with family.",
                Priority = 1
            };
            Event9 = new Event
            {
                UserId = 3,
                Time = new DateTime(2024, 11, 1, 10, 0, 0),
                Date = new DateTime(2024, 11, 1),
                NotifTime = new DateTime(2024, 10, 31, 10, 0, 0),
                Header = "Grocery Shopping",
                Description = "Weekly grocery shopping.",
                Priority = 1
            };
            Event10 = new Event
            {
                UserId = 1,
                Time = new DateTime(2024, 11, 10, 16, 0, 0),
                Date = new DateTime(2024, 11, 10),
                NotifTime = new DateTime(2024, 11, 9, 16, 0, 0),
                Header = "Project Deadline",
                Description = "Finalize the project for submission.",
                Priority = 2
            };

            // Создание настроек
            Settings1 = new Settings
            {
                UserId = 1,
                ThemeId = 1,
                LangId = "en-US"
            };
            Settings2 = new Settings
            {
                UserId = 2,
                ThemeId = 1,
                LangId = "ru"
            };
            Settings3 = new Settings
            {
                UserId = 3,
                ThemeId = 2,
                LangId = "en-US"
            };
        }

        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
#endif
}