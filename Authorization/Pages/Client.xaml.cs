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
        private string _role;
        private string gender = "";
        private string name = "";
        public Client(Authtorizations user ,string role)
        {
            InitializeComponent();
            if (DateTime.Now.Hour >= 19 && DateTime.Now.Minute > 0)
            {
                MessageBox.Show("Смена закончена!");
                NavigationService.GoBack();
            }
            else
            {
                _user = user;
                _role = role;
                NameTake();
                TimeCheck();

                if (role != null)
                {
                    _role = "Гость";
                    TextBlock_.Text = $"Вы вошли как {role}";
                }
                else
                {
                    TextBlock_.Text = "Вы вошли как пользователь!";
                }
            }
           
        }

        private void TextBlock_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void TimeCheck()
        {
            DateTime now = DateTime.Now;

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

        private void NameTake()
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
    }
}
