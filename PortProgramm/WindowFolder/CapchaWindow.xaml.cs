using PortProgramm.ClassFolder;
using PortProgramm.PageFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortProgramm.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для CapchaWindow.xaml
    /// </summary>
    public partial class CapchaWindow : Window
    {
        public bool IsReady = false;
        string _CaptchaCode;
        public CapchaWindow()
        {
            InitializeComponent();
        }

        private void CaphaText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CaphaImput.Focus();
        }

        private void CaphaImput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CaphaImput.Text) && CaphaImput.Text.Length > 0)
                CaphaText.Visibility = Visibility.Collapsed;
            else
                CaphaText.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CaphaImput.Text == _CaptchaCode)
            {
                IsReady = true;

                
                StartWindow startwindow = App.Current.Windows[0] as StartWindow;
                Type type = typeof(StartWindow);
                MethodInfo methodInfo2 = type.GetMethod("HideOverlay");
                if (methodInfo2 != null)
                {
                    methodInfo2.Invoke(startwindow, null);
                }
                RegistrationPage page = startwindow.MainFrame.Content as RegistrationPage;
                Type pagetype = typeof(RegistrationPage);
                MethodInfo info1 = pagetype.GetMethod("HideOverlay");
                if (info1 != null)
                {
                    info1.Invoke(page, null);
                }
                MethodInfo info = pagetype.GetMethod("AddUser");            
                if (info != null)
                {
                    info.Invoke(page, null);
                }

                DoubleAnimation fadeOutAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(1),
                    AutoReverse = false
                };
                fadeOutAnimation.Completed += (S, _) =>
                {
                    MethodInfo method = type.GetMethod("AuthBTN_Click1");
                    object[] param = { S, _};
                    if (method != null)
                    {
                        method.Invoke(startwindow, param);
                    }
                    Close();
                };
                MainBorder.BeginAnimation(Border.OpacityProperty, fadeOutAnimation);
                

            }
            else
            {
                int w = (int)CaptchaIMG.Width;
                int h = (int)CaptchaIMG.Height;
                var CaptchaCode = Captcha.GenerateCaptcha();
                _CaptchaCode = CaptchaCode;

                var res = Captcha.GenerateImage(w, h, CaptchaCode);

                Stream stream = new MemoryStream(res.CaptchaByteData);

                CaptchaIMG.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };
            MainBorder.BeginAnimation(Border.OpacityProperty, fadeOutAnimation);
            int w = (int)CaptchaIMG.Width;
            int h = (int)CaptchaIMG.Height;
            var CaptchaCode = Captcha.GenerateCaptcha();
            _CaptchaCode = CaptchaCode;

            var res = Captcha.GenerateImage(w, h, CaptchaCode);

            Stream stream = new MemoryStream(res.CaptchaByteData);

            CaptchaIMG.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            StartWindow startwindow = App.Current.Windows[0] as StartWindow;
            Type type = typeof(StartWindow);
            MethodInfo methodInfo = type.GetMethod("ShowOverlay");
            if (methodInfo != null)
            {
                methodInfo.Invoke(startwindow, null);
            }
            RegistrationPage page = startwindow.MainFrame.Content as RegistrationPage;
            Type pagetype = typeof(RegistrationPage);
            MethodInfo info = pagetype.GetMethod("ShowOverlay");
            if (info != null)
            {
                info.Invoke(page, null);
            }

        }
    }
}
