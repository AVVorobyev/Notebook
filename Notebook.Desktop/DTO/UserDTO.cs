namespace Notebook.Desktop.DTO
{
    internal class UserDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
}
