using Google.Apis.Auth.OAuth2;
using PortProgramm.PageFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using System.Net;
using Google.Apis.Oauth2.v2;

namespace PortProgramm.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private Grid overlayGrid;
        public StartWindow()
        {
            InitializeComponent();
            InitializeOverlay();
            MainFrame.Navigate(new AuthorizationPage());
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StartAnimation()
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };

            

            RegTB.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            CreateTB.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            fadeOutAnimation.Completed += FadeOutAnimation_Completed;
            AuthBTN.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {
            RegTB.Text = "Авторизация";
            CreateTB.Text = "Войти в существующую учетную запись";
            AuthBTN.Content = "Войти";
            AuthBTN.Click -= AuthBTN_Click;
            AuthBTN.Click += AuthBTN_Click1; ;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };

            RegTB.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
            CreateTB.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
            AuthBTN.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
        }

        public void AuthBTN_Click1(object sender, RoutedEventArgs e)
        {
            StartAnimation2();

            

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1) // Продолжительность анимации в секундах
            };
            // Запускаем анимацию
            Page currentPage = MainFrame.Content as Page;
            if (currentPage != null)
            {


                fadeInAnimation.Completed += (s, _) =>
                {
                    // Переход на новую страницу после завершения анимации
                    MainFrame.Navigate(new AuthorizationPage());
                };
                currentPage.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
            
        }

        private void AuthBTN_Click(object sender, RoutedEventArgs e)
        {
            StartAnimation();
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1) // Продолжительность анимации в секундах
            };
            // Запускаем анимацию
            Page currentPage = MainFrame.Content as Page;
            if (currentPage != null)
            {


                fadeInAnimation.Completed += (s, _) =>
                {
                    // Переход на новую страницу после завершения анимации
                    MainFrame.Navigate(new RegistrationPage());
                };
                currentPage.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
        }

        private void StartAnimation2()
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };



            RegTB.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            CreateTB.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            fadeOutAnimation.Completed += FadeOutAnimation_Completed2;
            AuthBTN.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed2(object sender, EventArgs e)
        {
            RegTB.Text = "Регистрация";
            CreateTB.Text = "Создайте учетную запись чтобы получить доступ";
            AuthBTN.Content = "Зарегистрироваться";
            AuthBTN.Click -= AuthBTN_Click1;
            AuthBTN.Click += AuthBTN_Click; ;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };

            RegTB.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
            CreateTB.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
            AuthBTN.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);
        }

        private void InitializeOverlay()
        {
            // Создаем новый Grid (сетку), который будет слоем затемнения
            
            overlayGrid = new Grid
            {
                Background = new SolidColorBrush(Colors.Black),
                Opacity = 0.0, // Прозрачность слоя (от 0.0 до 1.0)
                Visibility = Visibility.Collapsed, // Начально делаем слой невидимым
                IsHitTestVisible = true // Блокируем взаимодействие с элементами под слоем
            };

            // Добавляем слой затемнения в главную сетку окна
            LayoutRoot.Children.Add(overlayGrid);

            // Подписываемся на событие изменения размеров окна
            SizeChanged += (sender, e) => UpdateOverlaySize();
        }

        private void UpdateOverlaySize()
        {
            // Обновляем размеры слоя затемнения при изменении размеров окна
            overlayGrid.Width = ActualWidth;
            overlayGrid.Height = ActualHeight;
        }

        public void ShowOverlay()
        {
            // Показываем слой затемнения
            overlayGrid.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 0.5,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };
            overlayGrid.BeginAnimation(Grid.OpacityProperty, fadeInAnimation);
        }

        public void HideOverlay()
        {
            // Скрываем слой затемнения
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.5,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };
            fadeInAnimation.Completed += (S, _) =>
            {
                overlayGrid.Visibility = Visibility.Collapsed;
            };
            overlayGrid.BeginAnimation(Grid.OpacityProperty, fadeInAnimation);
            
        }
    }
}
