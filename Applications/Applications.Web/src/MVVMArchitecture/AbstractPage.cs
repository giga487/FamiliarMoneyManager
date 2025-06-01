using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Applications.Web.src.MVVMArchitecture
{
    public class AbstractPage<T> : ComponentBase where T : IViewModel
    {
        [Inject]
        public T ViewModel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ViewModel != null)
            {
                ViewModel.OnPropertyChanged += OnViewModelPropertyChanged;
            }
        }

        protected virtual void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

    }
}
