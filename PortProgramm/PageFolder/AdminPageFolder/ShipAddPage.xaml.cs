using Google.Apis.PeopleService.v1.Data;
using Microsoft.Win32;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.IO;
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
        byte[] shipdata = null;
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
                    ShipPhoto = shipdata,
                    ShipCompany = CompanyTb.textBox.Text,
                    CrewCount = int.Parse(CrewTb.textBox.Text)

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (.jpg,.jpeg, .png) |.jpg; .jpeg;.png";
            if (openFileDialog.ShowDialog() == true)
            {

                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = new Uri(openFileDialog.FileName);
                imageSource.EndInit();


                byte[] imageData;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    imageData = ms.ToArray();
                }


                //using (var context = DBEntities.GetContext())
                //{
                //    User reader = context.User.FirstOrDefault(r => r.IdUser == VariableClass.ReaderId);
                //    if (reader != null)
                //    {
                //        reader.PhotoReader = imageData;
                //        context.SaveChanges();
                //    }
                //}

                shipdata = imageData;

                //PhotoIm.Source = imageSource;
            }
        }
    }
}
