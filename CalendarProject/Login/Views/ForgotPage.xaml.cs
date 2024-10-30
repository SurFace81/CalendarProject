using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Net.Mail;
using System.Net;
using CalendarProject.EntityFramework;
using Windows.UI.Popups;

namespace CalendarProject.Views
{ 
    public sealed partial class ForgotPage : Page
    {
        DbWorker dbWorker;

        public ForgotPage()
        {
            this.InitializeComponent();

            dbWorker = App.GetService<DbWorker>();
        }

        private void GoPrevPage()
        {
            App.loginWindow.Content = App.GetService<LoginPage>();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoPrevPage();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            email_err.Visibility = Visibility.Collapsed;
            if (!SessionContext.ValidateEmail(emailTb.Text))
            {
                email_err.Visibility = Visibility.Visible;
                return;
            }

            User? user = dbWorker.DbExecuteSQL<User>("SELECT * FROM Users WHERE Email = @p0", emailTb.Text).FirstOrDefault();
            if (user != null)
            {
                int newPass = (new Random()).Next(100000, 999999);
                SendMail(emailTb.Text, newPass);

                user.Password = SessionContext.GetMD5Hash(newPass.ToString());
                dbWorker.DbUpdate<User>(user);

                infoText.Text = "Your new password has been sent to your email. Please check your spam folder";
            }
        }

        private async void SendMail(string To, int newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress("calendar.official@yandex.ru"),
                    Subject = "New password",
                    Body = "A new password for your account:\n" + newPassword.ToString()
                };
                mail.To.Add(new MailAddress(To));

                using (SmtpClient client = new SmtpClient("smtp.yandex.ru", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("calendar.official@yandex.ru", "oomatpwfsjcwzhco");
                    await client.SendMailAsync(mail);
                };

            }
            catch (Exception) { }
        }
    }
}
