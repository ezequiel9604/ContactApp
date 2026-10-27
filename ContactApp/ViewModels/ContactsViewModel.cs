using ContactApp.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contact = ContactApp.Models.Contact;

namespace ContactApp.ViewModels
{
    public partial class ContactsViewModel : ObservableObject
    {
        private readonly ContactService _service;
        public ObservableCollection<Contact> Contacts { get; } = [];

        public ContactsViewModel(ContactService service) => _service = service;

        [RelayCommand]
        async Task LoadAsync()
        {
            try
            {
                Contacts.Clear();
                foreach (var c in await _service.GetContactsAsync())
                    Contacts.Add(c);
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"---------------- {ex.ToString()}");
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
            }
        }
    }
}
