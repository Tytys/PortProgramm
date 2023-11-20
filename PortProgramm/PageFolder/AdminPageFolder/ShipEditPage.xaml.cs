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
    /// Логика взаимодействия для ShipEditPage.xaml
    /// </summary>
    public partial class ShipEditPage : Page
    {
        Ship ship = new Ship();
        byte[] shipdata = null;
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
                WeightTb.textBox.Text = WeightTb.Hint;
                NameTb.textBox.Text = NameTb.Hint;
                CountryTb.textBox.Text = CountryTb.Hint;
                CompanyTb.textBox.Text = CompanyTb.Hint;
                CrewTb.textBox.Text = CrewTb.Hint;
                int index = ShipTypeCb.SelectedIndex + 1;
                ship = DBEntities.GetContext().Ship   
                    .FirstOrDefault(u => u.ShipId == ship.ShipId);
                ship.ShipWeight = int.Parse(WeightTb.textBox.Text);
                ship.ShipName = NameTb.textBox.Text;
                ship.ShipTypeId = index;
                ship.ShipCountry = CountryTb.textBox.Text;
                ship.ShipPhoto = shipdata;
                ship.ShipCompany = CompanyTb.textBox.Text;
                ship.CrewCount = int.Parse(CrewTb.textBox.Text);
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
            CompanyTb.textBox.Text = CompanyTb.Hint;
            CrewTb.textBox.Text = CrewTb.Hint;
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
