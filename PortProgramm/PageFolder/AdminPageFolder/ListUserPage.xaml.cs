using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using PortProgramm.WindowFolder;
using PortProgramm.WindowFolder.AdminWindowFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortProgramm.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для ListUserPage.xaml
    /// </summary>
    public partial class ListUserPage : Page
    {
        private Grid overlayGrid;
        public ListUserPage()
        {
            InitializeComponent();
            DgUser.ItemsSource = DBEntities.GetContext().User
                .ToList().OrderBy(u => u.UserName);
            
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgUser.ItemsSource = DBEntities.GetContext()
                .User.Where(u => u.UserName
                .StartsWith(SearchTB.Text))
                .ToList().OrderBy(u => u.UserName);
            if (DgUser.Items.Count <= 0)
            {
                MBClass.ErrorMB("Данные не найдены");
            }
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e)
        {
            User user = DgUser.SelectedItem as User;

            if (DgUser.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите пользователя" +
                    " для удаления");
            }
            else
            {
                if (MBClass.QuestionMB("Удалить " +
                    $"пользователя с логином " +
                    $"{user.UserName}?"))
                {
                    DBEntities.GetContext().User
                        .Remove(DgUser.SelectedItem as User);
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("Пользователь удален");
                    DgUser.ItemsSource = DBEntities.GetContext()
                        .User.ToList().OrderBy(u => u.UserName);
                }

            }
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
                    new EditUserPage(DgUser.SelectedItem as User));
            }
        }

        private void AddMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserPage());
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridCell cell && cell.Column is DataGridTemplateColumn)
            {
                // Разрешить редактирование ячейки
                cell.IsEnabled = true;

                // Переключить фокус на ячейку
                cell.Focus();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DgUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new AdminQuestionWindow().ShowDialog();
        }

        public void ShowPassword()
        {
            PasswordDG.Visibility= Visibility.Visible;
        }

        
        
    }
}
