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
    /// Логика взаимодействия для ShelduleListPage.xaml
    /// </summary>
    public partial class ShelduleListPage : Page
    {
        public ShelduleListPage()
        {
            InitializeComponent();
            DgUser.ItemsSource = DBEntities.GetContext().Schedule
                .ToList().OrderBy(u => u.ArrivalPort);
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e)
        {
            Schedule schedule = DgUser.SelectedItem as Schedule;

            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите пользователя" +
                    " для удаления");
            }
            else
            {
                if (MBClass.QuestionMB("Удалить " +
                    $"расписание с временем отправления " +
                    $"{schedule.DepartureTime}?"))
                {
                    DBEntities.GetContext().Schedule
                        .Remove(DgUser.SelectedItem as Schedule);
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("Пользователь удален");
                    DgUser.ItemsSource = DBEntities.GetContext()
                        .Schedule.ToList().OrderBy(u => u.ArrivalPort);
                }

            }
        }

        private void AddMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShelduleAddPage());
        }

        private void EditMI_Click(object sender, RoutedEventArgs e)
        {
            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите " +
                    "расписание для редактирования");
            }
            else
            {
                NavigationService.Navigate(
                    new ShelduleEditPage(DgUser.SelectedItem as Schedule));
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgUser.ItemsSource = DBEntities.GetContext()
                .Schedule.Where(u => u.ArrivalPort
                .StartsWith(SearchTB.Text))
                .ToList().OrderBy(u => u.ArrivalPort);
            if (DgUser.Items.Count <= 0)
            {
                MBClass.ErrorMB("Данные не найдены");
            }
        }
    }
}
