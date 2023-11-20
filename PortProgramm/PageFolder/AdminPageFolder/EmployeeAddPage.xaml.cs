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
    /// Логика взаимодействия для EmployeeAddPage.xaml
    /// </summary>
    public partial class EmployeeAddPage : Page
    {
        byte[] employeedata = null;
        byte[] passportdata = null;
        public EmployeeAddPage()
        {
            InitializeComponent();
            PostCb.ItemsSource = DBEntities.GetContext()
               .Post.ToList();
            AdressCb.ItemsSource = DBEntities.GetContext()
               .Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext()
               .Gender.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Employee.Add(new Employee()
                {
                    EmployeeFIO = FIOTb.textBox.Text,
                    Phone = PhoneTb.textBox.Text,
                    PassportPhoto = passportdata,
                    PostId = int.Parse(PostCb.SelectedValue.ToString()),
                    Email = EmailTb.textBox.Text,
                    GenderId = int.Parse(GenderCb.SelectedValue.ToString()),
                    Photo = employeedata,
                    AdressId = int.Parse(AdressCb.SelectedValue.ToString())
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Сотрудник успешно добавлен");
                NavigationService.Navigate(new EmployeeListPage());
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

                passportdata = imageData;

                //PhotoIm.Source = imageSource;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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

                employeedata = imageData;

                //PhotoIm.Source = imageSource;
            }
        }
    }
}
