using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Authorization.Models;
using Authorization.Services;


namespace Authorization.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private Authtorizations _user;
        private DispatcherTimer timer;
        private DispatcherTimer _timer;
        private string _role;
        private string gender = "";
        private string name = "";
        public Client(Authtorizations user, string role)
        {
            InitializeComponent();
            _user = user;
            _role = role;
            StartTimer();
            StartSecondTimer();
            NameTake(user);
            Change();
            if (role != null)
            {
                TextBlock_.Text = $"Вы вошли как {role}";
            }
            else
            {
                TextBlock_.Text = "Вы вошли как гость!";
            }
        }

        private void TextBlock_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void StartTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void StartSecondTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += TimerTick_;
            timer.Start();
        }


        
        private void TimerTick_(object sender, EventArgs e)
        {
            timer.Stop();
            _timer.Stop();
            if (_role == "Администратор")
            {
                NavigationService.Navigate(new AdminPage());
            }
            else
            {
                NavigationService.Navigate(new ClientPage());
            }
        }

        
        private void TimerTick(object sender, EventArgs e)
        {
            Change();
        }


        private void Change() 
        {
            DateTime now = DateTime.Now;
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute > 0)
            {
                MessageBox.Show("Смена закончена!");
                NavigationService.GoBack();
                timer.Stop();   
            }
            else
            {
                if (now.Hour >= 10 && now.Hour <= 12)
                {
                    Time.Text = $"Доброе утро! {gender} {name}";
                }
                else if ((now.Hour >= 12 && now.Minute >= 1) && now.Hour <= 17)
                {
                    Time.Text = $"Добрый день! {gender} {name}";
                }
                else if ((now.Hour >= 17 && now.Minute >= 1) && now.Hour <= 19)
                {
                    Time.Text = $"Добрый вечер! {gender} {name}";
                }
            }
        }

        private void NameTake(Authtorizations user)
        {
            if (user.login != null)
            {
                construction_organizationEntities db = Helper.GetContext();
                var usernow = db.Users.Where(x => x.id == _user.user_id).FirstOrDefault();
                if (usernow != null)
                {
                    if (usernow.name[usernow.name.Length - 1].ToString() == "a" || usernow.name[usernow.name.Length - 1].ToString() == "я")
                    {
                        gender = "Mrs";
                        name = usernow.name;
                    }
                    else
                    {
                        gender = "Mr";
                        name = usernow.name;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка!");
                }
            }
            else
            {
                name = "Гость";
            }
            
        }
    }
}
