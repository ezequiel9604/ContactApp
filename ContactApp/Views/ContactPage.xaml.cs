using ContactApp.Models;
using Contact = ContactApp.Models.Contact;
using System.Collections.ObjectModel;

namespace ContactApp.Views;


public partial class ContactPage : ContentPage
{
	public ContactPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var contacts = new ObservableCollection<Contact>(ContactRepository.GetContacts());
        listContacts.ItemsSource = contacts;
    }

    public async void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        Contact selectedContact = new Contact((Contact)listContacts.SelectedItem);

		listContacts.SelectedItem = null;

       // await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={selectedContact.Id}");
    }

    public async void BtnAddContact_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddContactPage));
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
                    var contacts = new ObservableCollection<Contact>(ContactRepository.GetContacts());
                    listContacts.ItemsSource = contacts;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }

    public void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        var contacts = new ObservableCollection<Contact>(ContactRepository.SearchContacts(searchText));
        listContacts.ItemsSource = contacts;
    }


}