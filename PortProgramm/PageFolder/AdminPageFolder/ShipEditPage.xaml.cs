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
    /// Логика взаимодействия для ShipEditPage.xaml
    /// </summary>
    public partial class ShipEditPage : Page
    {
        Ship ship = new Ship();
        public ShipEditPage(Ship ship)
        {
            InitializeComponent();
            DataContext = ship;

            this.ship.ShipId = ship.ShipId;

            ShipTypeCb.ItemsSource = DBEntities.GetContext()
                .ShipType.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ShipTypeCb.SelectedIndex + 1;
                ship = DBEntities.GetContext().Ship   
                    .FirstOrDefault(u => u.ShipId == ship.ShipId);
                ship.ShipWeight = int.Parse(WeightTb.textBox.Text);
                ship.ShipName = NameTb.textBox.Text;
                ship.ShipTypeId = index;
                ship.ShipCountry = CountryTb.textBox.Text;              
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Данные успешно отредактированы");
                NavigationService.Navigate(new ShipListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WeightTb.textBox.Text = WeightTb.Hint;
            NameTb.textBox.Text = NameTb.Hint;
            CountryTb.textBox.Text = CountryTb.Hint;
        }
    }
}
