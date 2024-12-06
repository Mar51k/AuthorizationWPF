using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;
using Authorization.Pages;
using Authorization.Services;
using Authorization.Models;
using System.Windows.Threading;

namespace Authorization.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authoriz.xaml
    /// </summary>
    public partial class Authoriz : Page
    {
        int click;
        int false_captcha_count = 0;
        private DispatcherTimer timer;
        private TimeSpan timeRemaining;
        public Authoriz()
        {
            InitializeComponent();
            click = 0;
        }



        private void InitializeTimer()
        {
            timeRemaining = TimeSpan.FromSeconds(10);
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeRemaining > TimeSpan.Zero)
            {
                timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
                txtBlockTimer.Visibility = Visibility.Visible;
                txtBlockTimer.Text = timeRemaining.ToString(@"mm\:ss");
                loginbox.IsEnabled = false;
                pwdbox.IsEnabled = false;
                btnEnterGuests.IsEnabled = false;
                btnEnter.IsEnabled = false;
                txtboxCaptcha.IsEnabled = false;
            }
            else
            {
                txtBlockTimer.Visibility = Visibility.Hidden;
                loginbox.IsEnabled = true;
                pwdbox.IsEnabled = true;
                btnEnterGuests.IsEnabled = true;
                btnEnter.IsEnabled = true;
                txtboxCaptcha.IsEnabled = true;
            }
        }



        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, null));
        }

        private void GenerateCapcha()
        {
            txtBlockCaptcha.Visibility = Visibility.Visible;
            txtboxCaptcha.Visibility = Visibility.Visible;

            string captchatext = GenerateCapthcaText.Generate_CapthcaText(6);
            txtBlockCaptcha.Text = captchatext;
            txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = loginbox.Text.Trim();
            string password = pwdbox.Password.Trim();
            
            construction_organizationEntities db = Helper.GetContext();

            var user = db.Authtorizations.Where(x => x.login == login && x.password == password).FirstOrDefault();

            if (click == 1)
            {
                if (user != null)
                {
                    var staff = db.staff.Where(x => x.account == user.user_id).FirstOrDefault();
                    var role_ = db.roles.Where(x => x.id == staff.role).FirstOrDefault();
                    if (staff != null && role_ != null)
                    {
                        if (staff.role == role_.id)
                        {
                            if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour <= 18)
                            {
                                MessageBox.Show($"Вы вошли под: {role_.name}");
                                LoadPage(role_.name.ToString(), user);
                                click = 0;
                            }
                            else
                            {
                                if (DateTime.Now.Hour <= 10)
                                {
                                    MessageBox.Show("Смена ещё не началась!");
                                    click = 0;
                                }
                                else if(DateTime.Now.Hour >= 19)
                                {
                                    MessageBox.Show("Смена уже закончилась!");
                                    click = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        LoadPage(null, user);
                    }
                }
                else
                {
                    MessageBox.Show("Логин или пароль неверны");
                    GenerateCapcha();
                }   
            }
            else if(click > 1)
            {
                if (user != null && txtBlockCaptcha.Text == txtboxCaptcha.Text)
                {
                    var staff = db.staff.Where(x => x.account == user.user_id).FirstOrDefault();
                    var role_ = db.roles.Where(x => x.id == staff.role).FirstOrDefault();
                    if (staff != null && role_ != null)
                    {
                        if (staff.role == role_.id)
                        {
                            if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour <= 18)
                            {
                                MessageBox.Show($"Вы вошли под: {role_.name}");
                                LoadPage(role_.name.ToString(), user);
                                click = 0;
                            }
                            else
                            {
                                if (DateTime.Now.Hour <= 10)
                                {
                                    MessageBox.Show("Смена ещё не началась!");
                                    click = 0;
                                }
                                else if (DateTime.Now.Hour >= 19)
                                {
                                    MessageBox.Show("Смена уже закончилась!");
                                    click = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        LoadPage(null, user);
                    }
                }
                else
                {
                    MessageBox.Show("Введите данные заново");
                    false_captcha_count++;
                    GenerateCapcha();
                    if (false_captcha_count == 3)
                    {
                        InitializeTimer();
                        false_captcha_count = 0;
                    }
                }
            }
        }

        private void LoadPage(string _role, Authtorizations user)
        {
            click = 0;
            txtBlockCaptcha.Visibility = Visibility.Hidden;
            txtboxCaptcha.Visibility = Visibility.Hidden;
            NavigationService.Navigate(new Client(user, _role));
        }
    }
}
