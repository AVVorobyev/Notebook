using Newtonsoft.Json;
using Notebook.Core.Models;
using Notebook.Desktop.DTO;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Notebook.Desktop
{
    public class Repository
    {
        public static string CurrentUserName { get; private set; }
        public readonly static ObservableCollection<Contact> Contacts = new();
        private static string _url;
        private static string _token;

        static Repository() => InitializeConfiguration();
        

        private static void InitializeConfiguration()
        {
            var settings = ConfigurationManager.AppSettings;
            if (settings.Count > 0)
            {
                _url = settings["url"] ?? throw new ConfigurationErrorsException("Error reading app settings");
            }
        }

        public async Task<string> LogInAsync(string userName, string password)
        {
            string _result = null; var _contactJson = string.Empty;

            var _user = new User() { UserName = userName, Password = password };

            string _userJson = JsonConvert.SerializeObject(_user);

            var _request = WebRequest.Create(_url + "/token");
            _request.ContentType = "application/json";
            _request.Method = "POST";

            using (var sw = new StreamWriter(_request.GetRequestStream()))
            {
                await sw.WriteAsync(_userJson);
            }

            WebResponse _response = null;
            try
            {
                _response = await _request.GetResponseAsync();
            }
            catch (Exception e)
            {
                _result = e.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _contactJson = await sr.ReadToEndAsync();
            }

            UserDTO _userDTO = null;
            try
            {
                _userDTO = JsonConvert.DeserializeObject<UserDTO>(_contactJson);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            if (_userDTO == null) return _result;

            if (_userDTO.Succeeded) { CurrentUserName = userName; _token = _userDTO.Token; return null; };

            _result = _userDTO.ErrorMessage;

            return _result;
        }

        public async Task<string> RegistrationAsync(string userName, string password)
        {
            string _result = null; var _contactJson = string.Empty;

            var _user = new User() { UserName = userName, Password = password };

            string _userJson = JsonConvert.SerializeObject(_user);

            var request = WebRequest.Create(_url + "/registration");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                await sw.WriteAsync(_userJson);
            }

            WebResponse _response = null;
            try
            {
                _response = request.GetResponse();
            }
            catch (Exception e)
            {
                _result = e.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _contactJson = await sr.ReadToEndAsync();
            }

            UserDTO _userDTO = null;
            try
            {
                _userDTO = JsonConvert.DeserializeObject<UserDTO>(_contactJson);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            if (_userDTO == null) return _result;

            if (_userDTO.Succeeded) { CurrentUserName = userName; _token = _userDTO.Token; return null; };

            _result = _userDTO.ErrorMessage;

            return _result;
        }

        public async Task<string> GetContactsAsync(int takePages, int skipPages)
        {
            string _result = null; var _contactJson = string.Empty;

            var _request = (HttpWebRequest)WebRequest.Create(_url + $"/contact?skipPages={skipPages}&takePages={takePages}");
            _request.ContentType = "application/json";
            _request.Method = "GET";
            _request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            WebResponse _response = null;
            try
            {
                _response = await _request.GetResponseAsync();
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _contactJson = await sr.ReadToEndAsync();
            }

            Contacts.Clear();

            ContactDTO _contactDTO = null;
            try
            {
                _contactDTO = JsonConvert.DeserializeObject<ContactDTO>(_contactJson);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            if (_contactDTO.Succeeded)
                foreach (var contact in _contactDTO.Contacts)
                {
                    Contacts.Add(contact);
                }

            return _result;
        }


        public async Task<string> EditContactAsync(Contact contact)
        {
            string _result = null;
            string _contactJson = JsonConvert.SerializeObject(contact);

            var _request = (HttpWebRequest)WebRequest.Create(_url + "/contact");
            _request.Method = "PUT";
            _request.ContentType = "application/json";
            _request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            using (var sw = new StreamWriter(_request.GetRequestStream()))
            {
                await sw.WriteAsync(_contactJson);
            }

            WebResponse _response = null;
            try
            {
                _response = _request.GetResponse();

            }
            catch (Exception e)
            {
                _result = e.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _result = await sr.ReadToEndAsync();
            }

            ContactDTO _contactDTO = null;
            try
            {
                _contactDTO = JsonConvert.DeserializeObject<ContactDTO>(_result);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            if (_contactDTO == null) return _result;

            if (_contactDTO.Succeeded) return null;

            _result = _contactDTO.ErrorMessage;
            return _result;

        }

        public async Task<string> AddContactAsync(Contact contact)
        {
            var _result = string.Empty;
            var _contactJson = JsonConvert.SerializeObject(contact);

            var _request = (HttpWebRequest)WebRequest.Create(_url + "/contact");
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            using (var sw = new StreamWriter(_request.GetRequestStream()))
            {
                await sw.WriteAsync(_contactJson);
            }

            WebResponse _response = null;
            try
            {
                _response = await _request.GetResponseAsync();
            }
            catch (Exception e)
            {
                _result = e.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _result = await sr.ReadToEndAsync();
            }

            ContactDTO _contactDTO = null;
            try
            {
                _contactDTO = JsonConvert.DeserializeObject<ContactDTO>(_result);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }
            if (_contactDTO == null) return _result;

            if (_contactDTO.Succeeded) return null;// GetContacts(); 
            _result = _contactDTO.ErrorMessage;

            return _result;
        }


        public async Task<string> DeleteContact(Contact contact)
        {
            var _result = string.Empty;
            var _contactJson = JsonConvert.SerializeObject(contact);

            var _request = (HttpWebRequest)WebRequest.Create(_url + "/contact");
            _request.Method = "DELETE";
            _request.ContentType = "application/json";
            _request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            using (var sw = new StreamWriter(_request.GetRequestStream()))
            {
                await sw.WriteAsync(_contactJson);
            }

            WebResponse _response = null;
            try
            {
                _response = await _request.GetResponseAsync();
            }
            catch (Exception e)
            {
                _result = e.Message;
            }

            if (_response == null) return _result;

            using (var sr = new StreamReader(_response.GetResponseStream()))
            {
                _result = await sr.ReadToEndAsync();
            }

            ContactDTO _contactDTO = null;
            try
            {
                _contactDTO = JsonConvert.DeserializeObject<ContactDTO>(_result);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }
            if (_contactDTO == null) return _result;

            if (_contactDTO.Succeeded) return null;
            _result = _contactDTO.ErrorMessage;

            return _result;
        }
    }
}
