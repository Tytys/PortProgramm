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
    /// Логика взаимодействия для EmployeeListPage.xaml
    /// </summary>
    public partial class EmployeeListPage : Page
    {
        public EmployeeListPage()
        {
            InitializeComponent();
            DgUser.ItemsSource = DBEntities.GetContext().Employee
                .ToList().OrderBy(u => u.EmployeeFIO);
        }

        private void EditMI_Click(object sender, RoutedEventArgs e)
        {
            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите " +
                    "сотрудника для редактирования");
            }
            else
            {
                NavigationService.Navigate(
                    new EmployeeEditPage(DgUser.SelectedItem as Employee));
            }
        }

        private void AddMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeeAddPage());
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = DgUser.SelectedItem as Employee;

            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите сотрудника" +
                    " для удаления");
            }
            else
            {
                if (MBClass.QuestionMB("Удалить " +
                    $"сотрудника с ФИО " +
                    $"{employee.EmployeeFIO}?"))
                {
                    foreach (Schedule item in DBEntities.GetContext().Schedule.ToList())
                    {
                        if (item.EmployeeId == employee.EmployeeId)
                        {
                            item.EmployeeId = null;
                        }
                    }
                    DBEntities.GetContext().Employee
                        .Remove(DgUser.SelectedItem as Employee);
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("Сотрудник удален");
                    DgUser.ItemsSource = DBEntities.GetContext()
                        .Employee.ToList().OrderBy(u => u.EmployeeFIO);
                }

            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgUser.ItemsSource = DBEntities.GetContext()
                .Employee.Where(u => u.EmployeeFIO
                .StartsWith(SearchTB.Text))
                .ToList().OrderBy(u => u.EmployeeFIO);
            if (DgUser.Items.Count <= 0)
            {
                MBClass.ErrorMB("Данные не найдены");
            }
        }

        private void AddPostMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PostAddPage());
        }

        private void InfoMI_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
