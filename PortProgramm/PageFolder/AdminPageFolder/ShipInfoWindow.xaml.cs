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
    /// Логика взаимодействия для ShipInfoWindow.xaml
    /// </summary>
    public partial class ShipInfoWindow : Window
    {
        Ship ship = new Ship();
        public ShipInfoWindow(Ship ship)
        {
            InitializeComponent();
            DataContext = ship;
            this.ship.ShipId = ship.ShipId;
            ShipImg.Source = ImageClass.ConvertByteArrayToImage(DBEntities.GetContext().Ship.Where(x => x.ShipId == this.ship.ShipId).First().ShipPhoto);
            ShipTypeCb.ItemsSource = DBEntities.GetContext()
               .ShipType.ToList();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
