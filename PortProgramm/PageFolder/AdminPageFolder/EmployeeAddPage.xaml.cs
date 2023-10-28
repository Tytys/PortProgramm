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
    /// Логика взаимодействия для EmployeeAddPage.xaml
    /// </summary>
    public partial class EmployeeAddPage : Page
    {
        public EmployeeAddPage()
        {
            InitializeComponent();
            PostCb.ItemsSource = DBEntities.GetContext()
               .Post.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = PostCb.SelectedIndex + 1;
                DBEntities.GetContext().Employee.Add(new Employee()
                {
                    EmployeeFIO = FIOTb.textBox.Text,
                    Phone = PhoneTb.textBox.Text,
                    Passport = int.Parse(PassportTb.textBox.Text),
                    PostId = index,
                    Adress= AdressTb.textBox.Text
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Сотрудник успешно добавлен");
                NavigationService.Navigate(new EmployeeListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                throw;
            }
        }
    }
}
