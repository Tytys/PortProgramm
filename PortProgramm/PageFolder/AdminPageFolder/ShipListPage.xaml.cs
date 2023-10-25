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
    /// Логика взаимодействия для ShipListPage.xaml
    /// </summary>
    public partial class ShipListPage : Page
    {
        public ShipListPage()
        {
            InitializeComponent();
            DgUser.ItemsSource = DBEntities.GetContext().Ship
                .ToList().OrderBy(u => u.ShipName);
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgUser.ItemsSource = DBEntities.GetContext()
                .Ship.Where(u => u.ShipName
                .StartsWith(SearchTB.Text))
                .ToList().OrderBy(u => u.ShipName);
            if (DgUser.Items.Count <= 0)
            {
                MBClass.ErrorMB("Данные не найдены");
            }
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e)
        {
            Ship user = DgUser.SelectedItem as Ship;

            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите корабль" +
                    " для удаления");
            }
            else
            {
                if (MBClass.QuestionMB("Удалить " +
                    $"корабль с именем " +
                    $"{user.ShipName}?"))
                {
                    DBEntities.GetContext().Ship
                        .Remove(DgUser.SelectedItem as Ship);
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("корабль удален");
                    DgUser.ItemsSource = DBEntities.GetContext()
                        .Ship.ToList().OrderBy(u => u.ShipName);
                }

            }
        }

        private void AddMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShipAddPage());
        }

        private void EditMI_Click(object sender, RoutedEventArgs e)
        {
            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите " +
                    "пользователя для редактирования");
            }
            else
            {
                NavigationService.Navigate(
                    new ShipEditPage(DgUser.SelectedItem as Ship));
            }
        }

        private void AddTypeMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShipAddTypePage());
        }
    }
}
