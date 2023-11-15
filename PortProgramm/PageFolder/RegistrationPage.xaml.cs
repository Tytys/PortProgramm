
using Newtonsoft.Json;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using PortProgramm.WindowFolder;
using PortProgramm.WindowFolder.UserWindowFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
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

namespace PortProgramm.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        const string clientID = "425393651327-j8vnjksm82kldngajmta94q3ojaa2dg1.apps.googleusercontent.com";
        const string clientSecret = "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";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        private Grid overlayGrid;
        public RegistrationPage()
        {
            InitializeComponent();
            InitializeOverlay();
            Loaded += RegistrationPage_Loaded;
        }

        private void RegistrationPage_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1) // Продолжительность анимации в секундах
            };

            // Запускаем анимацию
            MainBorder.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
                textPassword.Visibility = Visibility.Collapsed;
            else
                textPassword.Visibility = Visibility.Visible;
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {            
                new CapchaWindow().ShowDialog();             
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        public void AddUser()
        {
            if (!DBEntities.GetContext().User.Any(x => x.UserName == txtEmail.Text))
            {
                MBClass.ErrorMB("Логин уже существует");
            }
            else
            {

                DBEntities.GetContext().User.Add(new User()
                {
                    UserName = txtEmail.Text,
                    UserPassword = passwordBox.Password,
                    RoleId = 4
                });

                DBEntities.GetContext().SaveChanges();

                MBClass.ShowMesagePopup("Вы успешно зарегестрировались", App.Current.Windows[0]);
            }
        }



        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
                textEmail.Visibility = Visibility.Collapsed;
            else
                textEmail.Visibility = Visibility.Visible;
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void passwordBoxPasswordDoub_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBoxPasswordDoub.Password) && passwordBoxPasswordDoub.Password.Length > 0)
                textPasswordDoub.Visibility = Visibility.Collapsed;
            else
                textPasswordDoub.Visibility = Visibility.Visible;
        }

        private void textPasswordDoub_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBoxPasswordDoub.Focus();
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Generates state and PKCE values.
            string state = randomDataBase64url(32);
            string code_verifier = randomDataBase64url(32);
            string code_challenge = base64urlencodeNoPadding(sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());


            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);

            http.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                authorizationEndpoint,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                state,
                code_challenge,
                code_challenge_method);

            // Opens request in the browser.
            System.Diagnostics.Process.Start(authorizationRequest);

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync();

            // Brings this app back to the foreground.


            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                http.Stop();
                Console.WriteLine("HTTP server stopped.");
            });

            // Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {

                return;
            }
            if (context.Request.QueryString.Get("code") == null
                || context.Request.QueryString.Get("state") == null)
            {

                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incoming_state != state)
            {

                return;
            }


            // Starts the code exchange at the Token Endpoint.
            performCodeExchange(code, code_verifier, redirectURI);
            foreach (Window windows in App.Current.Windows)
            {
                if (windows.Title == "StartWindow")
                {
                    new UserWindow().Show();
                    windows.Close();
                }
            }
        }

        async void performCodeExchange(string code, string code_verifier, string redirectURI)
        {


            // builds the  request
            string tokenRequestURI = "https://www.googleapis.com/oauth2/v4/token";
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier,
                clientSecret
                );

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            Stream stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length);
            stream.Close();

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync();

                    // converts to dictionary
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    string access_token = tokenEndpointDecoded["access_token"];
                    userinfoCall(access_token);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync();
                        }
                    }

                }
            }
        }


        async void userinfoCall(string access_token)
        {

            // builds the  request
            string userinfoRequestURI = "https://www.googleapis.com/oauth2/v3/userinfo";

            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestURI);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", access_token));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
            }
        }


        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        public static string randomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return base64urlencodeNoPadding(bytes);
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        /// <returns></returns>
        public static byte[] sha256(string inputStirng)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
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
