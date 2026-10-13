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

        await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={selectedContact.Id}");

    }

}