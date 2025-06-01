using System.ComponentModel;

namespace Applications.Web.src.MVVMArchitecture.ModelView
{
    public class AccountMV: IViewModel
    {
        public event EventHandler<PropertyChangedEventArgs> OnPropertyChanged;
        public string ClientId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public AccountMV()
        {
            // Initialize properties or perform any setup if needed
        }
        // Additional methods or properties can be added here as needed
    }
}
