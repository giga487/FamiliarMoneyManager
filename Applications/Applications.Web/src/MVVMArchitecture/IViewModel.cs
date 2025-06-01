using System.ComponentModel;

namespace Applications.Web.src.MVVMArchitecture
{
    public interface IViewModel
    {
        public event EventHandler<PropertyChangedEventArgs> OnPropertyChanged;
    }
}
