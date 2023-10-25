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

namespace PortProgramm.PageFolder.UserPageFolder
{
    /// <summary>
    /// Логика взаимодействия для SheduleUserListPage.xaml
    /// </summary>
    public partial class SheduleUserListPage : Page
    {
        public SheduleUserListPage()
        {
            InitializeComponent();
            DgUser.ItemsSource = DBEntities.GetContext().Schedule
                .ToList().OrderBy(u => u.ArrivalPort);
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
