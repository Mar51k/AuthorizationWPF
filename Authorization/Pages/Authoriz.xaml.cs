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
using Authorization.Pages;
using Authorization.Services;
using Authorization.Models;

namespace Authorization.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authoriz.xaml
    /// </summary>
    public partial class Authoriz : Page
    {
        int click;
        public Authoriz()
        {
            InitializeComponent();
            click = 0;
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
                            MessageBox.Show($"Вы вошли под: {role_.name}");
                            LoadPage(role_.name.ToString(), user);
                            click = 0;
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
                            MessageBox.Show($"Вы вошли под: {role_.name}");
                            LoadPage(role_.name.ToString(), user);
                            click = 0;

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
