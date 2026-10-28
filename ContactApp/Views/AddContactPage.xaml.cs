using ContactApp.Models;
using Contact = ContactApp.Models.Contact;
using ContactApp.ViewModels;

namespace ContactApp.Views;

public partial class AddContactPage : ContentPage
{
    private readonly ContactsViewModel _viewModel;
    public AddContactPage(ContactsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
	}

    private async void BtnSave_Clicked(object sender, EventArgs e)
    {
        var newContact = new Contact(contactControl.Name, contactControl.Phone, contactControl.Email, contactControl.Address);

        var existingContacts = _viewModel.Contacts.Where(c => c.Name.ToLower() == newContact.Name.ToLower()).ToList();

        if (existingContacts.Count > 0)
        {
            await DisplayAlertAsync("Error", "A contact with the same name already exists.", "OK");
            return;
        }

        var isAdded = await _viewModel.AddContact(newContact);
        if (isAdded)
        {
            await DisplayAlertAsync("Success", "Contact added successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }

    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    private void ContactControl_OnError(object sender, string errorMessage)
    {
        DisplayAlertAsync("Error", errorMessage, "OK");
    }
}