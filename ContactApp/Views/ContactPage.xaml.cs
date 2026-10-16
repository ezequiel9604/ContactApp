using ContactApp.Models;
using Contact = ContactApp.Models.Contact;
using System.Collections.ObjectModel;

namespace ContactApp.Views;

public partial class ContactPage : ContentPage
{

    private bool _isSwiping;
    private CancellationTokenSource searchBarCct;

    public ContactPage()
	{
		InitializeComponent();
	}

    private void LoadContacts(List<Contact> contactsList)
    {
        var contacts = new ObservableCollection<Contact>(contactsList);
        listContacts.ItemsSource = contacts;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadContacts(ContactRepository.GetContacts());
    }

    //  public async void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //  {
    //      if (e.CurrentSelection.Count == 0) return;

    //      Contact selectedContact = new Contact((Contact)listContacts.SelectedItem);

    //listContacts.SelectedItem = null;

    //     await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={selectedContact.Id}");
    //  }

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