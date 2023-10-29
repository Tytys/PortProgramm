using Google.Apis.PeopleService.v1.Data;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
    /// Логика взаимодействия для ShelduleEditPage.xaml
    /// </summary>
    public partial class ShelduleEditPage : Page
    {
        Schedule scheldule = new Schedule();
        public ShelduleEditPage(Schedule schedule)
        {
            InitializeComponent();
            DataContext = schedule;

            this.scheldule.SheduleId = scheldule.SheduleId;
            ShipCb.ItemsSource = DBEntities.GetContext()
               .Ship.ToList();
            EmployeeCb.ItemsSource = DBEntities.GetContext()
               .Employee.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ShipCb.SelectedIndex + 1;
                int index2 = EmployeeCb.SelectedIndex + 1;
                scheldule = DBEntities.GetContext().Schedule
                    .FirstOrDefault(u => u.SheduleId == scheldule.SheduleId);
                scheldule.ShipId = index;
                scheldule.DeparturePort = SudaPort.textBox.Text;
                scheldule.ArrivalPort = TudaPost.textBox.Text;
                scheldule.DepartureTime = DateTime.Parse(SudaTime.datePiker.Text);
                scheldule.ArrivalTime = DateTime.Parse(TudaTime.datePiker.Text);
                scheldule.EmployeeId = index2;
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Данные успешно отредактированы");
                NavigationService.Navigate(new ShelduleListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SudaPort.textBox.Text = SudaPort.Hint;
            TudaPost.textBox.Text = TudaPost.Hint;
            SudaTime.datePiker.Text = SudaTime.Hint;
            TudaTime.datePiker.Text = TudaTime.Hint;
        }
    }
}
