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

        public ContactsViewModel(ContactService service)
        {
            _service = service;
        }

        [RelayCommand]
        async Task LoadAsync()
        {
            try
            {
                var contacts = await _service.GetContactsAsync();
                Contacts.Clear();

                foreach (var contact in contacts)
                {
                    Contacts.Add(contact);
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"---------------- {ex}", "CONNECTION");
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
            }
        }

        public async Task GetContactByName(string name)
        {
            try
            {
                
                var contacts = await _service.GetContactByNameAsync(name);
                Contacts.Clear();
                foreach (var c in contacts)
                {
                    Contacts.Add(c);
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"---------------- {ex}","CONNECTION");
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
            }
        }
    }
}
