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
using System.Windows.Shapes;

namespace PortProgramm.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для EmployeeInfoWindow.xaml
    /// </summary>
    public partial class EmployeeInfoWindow : Window
    {
        Employee employee = new Employee();
        public EmployeeInfoWindow(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;
            this.employee.EmployeeId = employee.EmployeeId;
            PassportImg.Source = ImageClass.ConvertByteArrayToImage(DBEntities.GetContext().Employee.Where(x => x.EmployeeId == this.employee.EmployeeId).First().PassportPhoto);
            EmployeeImg.Source = ImageClass.ConvertByteArrayToImage(DBEntities.GetContext().Employee.Where(x => x.EmployeeId == this.employee.EmployeeId).First().Photo);
            PostCb.ItemsSource = DBEntities.GetContext()
               .Post.ToList();
            AdressCb.ItemsSource = DBEntities.GetContext()
               .Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext()
               .Gender.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
