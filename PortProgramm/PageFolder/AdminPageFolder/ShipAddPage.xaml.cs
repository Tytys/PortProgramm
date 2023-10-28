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
    /// Логика взаимодействия для ShipAddPage.xaml
    /// </summary>
    public partial class ShipAddPage : Page
    {
        public ShipAddPage()
        {
            InitializeComponent();
            ShipTypeCb.ItemsSource = DBEntities.GetContext()
                .ShipType.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ShipTypeCb.SelectedIndex + 1;
                DBEntities.GetContext().Ship.Add(new Ship()
                {
                    ShipWeight = int.Parse(WeightTb.textBox.Text),
                    ShipName = NameTb.textBox.Text,
                    ShipTypeId = index,
                    ShipCountry = CountryTb.textBox.Text,
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Корабль успешно добавлен");
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
