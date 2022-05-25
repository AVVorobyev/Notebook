using System.Windows;

namespace Notebook.Desktop
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private MainWindow _mainWindow;
        private readonly Repository _repository;

        public AuthorizationWindow()
        {
            InitializeComponent();
            _repository = new Repository();
        }

        private async void ButtonClickEnter(object sender, RoutedEventArgs e)
        {
            if (userNameAuthorization.Text.Length < 6 || passwordAuthorization.Password.Length < 6)
            {
                MessageBox.Show("Error: Username or password is less than 6 characters long");
                return;
            }

            var _result = await _repository.LogInAsync(userNameAuthorization.Text, passwordAuthorization.Password);

            if (_result == string.Empty || _result == null)
            {
                _mainWindow = new MainWindow();
                _mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show(_result);
            }
        }

        private async void ButtonClickRegister(object sender, RoutedEventArgs e)
        {
            if (userNameRegistration.Text.Length < 6 || passRegistration.Password.Length < 6 ||
                passConfirm.Password.Length < 6)
            {
                MessageBox.Show("Error: Username or password is less than 6 characters long"); return;
            }

            if (passRegistration.Password != passConfirm.Password)
            {
                MessageBox.Show("Error: Passwords don't match"); return;
            }
            var _result = await _repository.RegistrationAsync(
                userNameRegistration.Text, passRegistration.Password);

            if (_result == string.Empty || _result == null)
            {
                await _repository.LogInAsync(userNameRegistration.Text, passRegistration.Password);
                _mainWindow = new MainWindow();
                _mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show(_result);
            }
        }
    }
}
