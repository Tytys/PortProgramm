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
    /// Логика взаимодействия для ShelduleAddPage.xaml
    /// </summary>
    public partial class ShelduleAddPage : Page
    {
        public ShelduleAddPage()
        {
            InitializeComponent();
            ShipCb.ItemsSource = DBEntities.GetContext()
               .Ship.ToList();
            EmployeeCb.ItemsSource = DBEntities.GetContext()
               .Employee.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Schedule.Add(new Schedule()
                {
                    ShipId = int.Parse(ShipCb.SelectedValue.ToString()),
                    DeparturePort = SudaPort.textBox.Text,
                    ArrivalPort = TudaPost.textBox.Text,
                    DepartureTime = DateTime.Parse(SudaTime.datePiker.Text),
                    ArrivalTime = DateTime.Parse(TudaTime.datePiker.Text),
                    EmployeeId = int.Parse(EmployeeCb.SelectedValue.ToString())
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Расписание успешно добавлено");
                NavigationService.Navigate(new ShelduleListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                throw;
            }
        }
    }
}
