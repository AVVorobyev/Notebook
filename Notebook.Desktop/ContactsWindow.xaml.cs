using Notebook.Core.Models;
using System.Windows;

namespace Notebook.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ContactsWindow : Window
    {        
        private Contact _selectedContact;
        private AuthorizationWindow _authorizationWindow;
        private readonly Repository _repository;
        private const string _errorMessageNumberTextBox = "The text field \"Number\" must contain only integers.";
        private const int _pagesRange = 25;
        private int _currentPage;

        public ContactsWindow()
        {
            InitializeComponent();
            _currentPage = 0;
            _repository = new Repository();
            InitializeContacs();
        }

        private async void InitializeContacs()
        {
            await _repository.GetContactsAsync(_pagesRange, 0);
            gridViewContacts.DataContext = Repository.Contacts;
            currentUserName.Text = Repository.CurrentUserName;
        }        

        private async void BtnClick_EditContact(object sender, RoutedEventArgs e)
        {
            _selectedContact = gridViewContacts.SelectedItem as Contact;

            if (_selectedContact == null) return;

            if (!CheckNumber(numberToEdit.Text)) { MessageBox.Show(_errorMessageNumberTextBox); return; }

            _selectedContact.Number = long.Parse(numberToEdit.Text);
            _selectedContact.Name = nameToEdit.Text;
            _selectedContact.Surname = surnameToEdit.Text;
            _selectedContact.Description = descriptionToEdit.Text;

            var _result = await _repository.EditContactAsync(_selectedContact);

            await _repository.GetContactsAsync(_pagesRange, _currentPage);

            if (string.IsNullOrEmpty(_result)) return;

            MessageBox.Show(_result);
        }

        private async void BtnClick_AddContact(object sender, RoutedEventArgs e)
        {
            if (!CheckNumber(numberToAdd.Text)) { MessageBox.Show(_errorMessageNumberTextBox); return; }

            _selectedContact = new Contact()
            {
                Number = long.Parse(numberToAdd.Text),
                Name = nameToAdd.Text,
                Surname = surnameToAdd.Text,
                Description = descriptionToAdd.Text
            };

            var _result = await _repository.AddContactAsync(_selectedContact);

            await _repository.GetContactsAsync(_pagesRange, _currentPage);

            if (string.IsNullOrEmpty(_result)) return;

            MessageBox.Show(_result);
        }

        private bool CheckNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            foreach (var item in str.ToCharArray())
                if (!char.IsDigit(item)) return false;

            return true;
        }

        private async void BtnClick_DeleteContact(object sender, RoutedEventArgs e)
        {
            _selectedContact = gridViewContacts.SelectedItem as Contact;

            if (_selectedContact == null) return;

            var _result = await _repository.DeleteContact(_selectedContact);

            await _repository.GetContactsAsync(_pagesRange, _currentPage);

            if (string.IsNullOrEmpty(_result)) return;
            MessageBox.Show(_result);
        }

        private void BtnClick_Logout(object sender, RoutedEventArgs e)
        {
            _authorizationWindow = new AuthorizationWindow();
            _authorizationWindow.Show();
            Close();
        }

        private async void BtnClick_Back(object sender, RoutedEventArgs e)
        {
            _currentPage -= _pagesRange;

            if (_currentPage < 0) _currentPage = 0;

            var _result = await _repository.GetContactsAsync(_pagesRange, _currentPage);

            if (string.IsNullOrEmpty(_result)) return;
            MessageBox.Show(_result);
        }

        private async void BtnClick_Forward(object sender, RoutedEventArgs e)
        {
            if (gridViewContacts.Items.Count < _pagesRange) return;
            _currentPage += _pagesRange;

            var _result = await _repository.GetContactsAsync(_pagesRange, _currentPage);

            if (string.IsNullOrEmpty(_result)) return;
            MessageBox.Show(_result);
        }
    }
}
