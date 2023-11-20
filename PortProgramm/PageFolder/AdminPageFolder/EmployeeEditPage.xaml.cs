using Google.Apis.PeopleService.v1.Data;
using Microsoft.Win32;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для EmployeeEditPage.xaml
    /// </summary>
    public partial class EmployeeEditPage : Page
    {
        Employee employee = new Employee();
        byte[] employeedata = null;
        byte[] passportdata = null;
        public EmployeeEditPage(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;

            this.employee.EmployeeId = employee.EmployeeId;

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
                FIOTb.textBox.Text = FIOTb.Hint;
                PhoneTb.textBox.Text = PhoneTb.Hint;
                EmailTb.textBox.Text = EmailTb.Hint;
                employee = DBEntities.GetContext().Employee
                    .FirstOrDefault(u => u.EmployeeId == employee.EmployeeId);
                employee.EmployeeFIO = FIOTb.textBox.Text;
                employee.Phone = PhoneTb.textBox.Text;
                employee.PassportPhoto = passportdata;
                employee.PostId = int.Parse(PostCb.SelectedValue.ToString());
                employee.Email = EmailTb.textBox.Text;
                employee.GenderId = int.Parse(GenderCb.SelectedValue.ToString());
                employee.Photo = employeedata;
                employee.AdressId = int.Parse(AdressCb.SelectedValue.ToString());
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Данные успешно отредактированы");
                NavigationService.Navigate(new EmployeeListPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FIOTb.textBox.Text = FIOTb.Hint;
            PhoneTb.textBox.Text = PhoneTb.Hint;
            EmailTb.textBox.Text = EmailTb.Hint;
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
