using Google.Apis.PeopleService.v1.Data;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
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

namespace PortProgramm.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для EmployeeEditPage.xaml
    /// </summary>
    public partial class EmployeeEditPage : Page
    {
        Employee employee = new Employee();
        public EmployeeEditPage(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;

            this.employee.EmployeeId = employee.EmployeeId;

            PostCb.ItemsSource = DBEntities.GetContext()
                .Post.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = PostCb.SelectedIndex + 1;
                employee = DBEntities.GetContext().Employee
                    .FirstOrDefault(u => u.EmployeeId == employee.EmployeeId);
                employee.EmployeeFIO = FIOTb.textBox.Text;
                employee.Phone = PhoneTb.textBox.Text;
                employee.Passport = int.Parse(PassportTb.textBox.Text);
                employee.PostId = index;
                employee.Adress = AdressTb.textBox.Text;
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Данные успешно отредактированы");
                NavigationService.Navigate(new EmployeeListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FIOTb.textBox.Text = FIOTb.Hint;
            PhoneTb.textBox.Text = PhoneTb.Hint;
            PassportTb.textBox.Text = PassportTb.Hint;
            AdressTb.textBox.Text = AdressTb.Hint;
        }
    }
}
