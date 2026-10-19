using ContactApp.Models;
using Contact = ContactApp.Models.Contact;
using System.Collections.ObjectModel;
using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace ContactApp.Views;

public partial class ContactPage : ContentPage
{

    private bool _isSwiping;
    private CancellationTokenSource searchBarCct;

    public ContactPage()
	{
		InitializeComponent();
        searchBarCct = new CancellationTokenSource();
    }

    private async IAsyncEnumerable<Communication.Contact> GetPhoneContacts()
    {
        try
        {
            var statusRequest = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

            if (statusRequest != PermissionStatus.Granted)
            {
                statusRequest = await Permissions.RequestAsync<Permissions.ContactsRead>();
            }

            if (statusRequest != PermissionStatus.Granted)
            {
                await DisplayAlertAsync("Permission Denied",
                    "Access to contacts is denied. Please enable it in settings.", "OK");
                yield break;
            }

            var phoneContacts = await Communication.Contacts.Default.GetAllAsync();

            if (phoneContacts == null)
                yield break;

            foreach (var contact in phoneContacts)
            {
                yield return contact;
            }
        }
        finally { };
    }

    private async void LoadContacts(List<Contact> contactsList)
    {
        var contacts = new ObservableCollection<Contact>(contactsList);

        try
        {

            var phoneContacts = GetPhoneContacts(); ;

            await foreach (var contact in phoneContacts)
            {
                string name = $"{contact.GivenName} {contact.FamilyName}".Trim();
                string phoneNumber = contact.Phones.FirstOrDefault()?.PhoneNumber ?? string.Empty;

                if (contacts.FirstOrDefault(x => x.PhoneNumber == phoneNumber) != null)
                {
                    continue;
                }

                Contact newContact = new(name, phoneNumber);

                ContactRepository.AddContact(newContact);
                contacts.Add(newContact);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Ok");
        }

        listContacts.ItemsSource = contacts.Where(x => !x.IsDeleted).ToList();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadContacts(ContactRepository.GetContacts());
    }

    private void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
    {
        _isSwiping = true;
    }

    public async void BtnAddContact_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddContactPage));
    }

    public async void OnContactTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject view && view.BindingContext is Contact contact)
        {
            await Shell.Current.GoToAsync($"{nameof(DetailsContactPage)}?Id={contact.Id}");
        }
    }

    public void OnEditSwipeInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Contact contact)
        {
            Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={contact.Id}");
        }
    }

    public void OnDeleteSwipeInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Contact contact)
        {

            DisplayAlertAsync("Delete Contact", $"Are you sure you want to delete {contact.Name}?", "Yes", "No").ContinueWith(async t =>
            {
                if (t.Result)
                {
                    ContactRepository.DeleteContact(contact.Id);
                    LoadContacts(ContactRepository.GetContacts());
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }

    public async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            searchBarCct?.Cancel();
            searchBarCct = new CancellationTokenSource();
            var token = searchBarCct.Token;

            await Task.Delay(500, token);

            string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
            LoadContacts(ContactRepository.SearchContacts(searchText));
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}