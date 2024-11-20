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

namespace Authorization.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private Authtorizations _user;
        private string _role;
        public Client(Authtorizations user ,string role)
        {
            InitializeComponent();
            _user = user;
            _role = role;
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

        private void TextBlock_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }
    }
}
