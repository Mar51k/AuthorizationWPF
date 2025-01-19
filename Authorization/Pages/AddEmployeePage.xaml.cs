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
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        public AddEmployeePage()
        {
            InitializeComponent();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                construction_organizationEntities db = Helper.GetContext();
                staff employee = new staff();
                employee.name = namebox.Text;
                employee.surname = surnambox.Text;
                if (patronymicbox.Text != null)
                {
                    employee.patronimyc = patronymicbox.Text;
                }
                else
                {
                    employee.patronimyc = "";
                }
                string txtrole = rolebox.Text;
                var role = db.roles.Where(x => x.name == txtrole).FirstOrDefault();
                employee.role = role.id;

                employee.passport_num = int.Parse(passportbox.Text);
                employee.inn_num = int.Parse(innbox.Text);
                employee.at_site = false;

                db.staff.Add(employee);
                await db.SaveChangesAsync();

                NavigationService.Navigate(new AdminPage());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }


        private void addToDB(staff empployee)
        {
            
            
            
        }
    }
}
