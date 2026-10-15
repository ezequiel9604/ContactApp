using ContactApp.Models;
using Contact = ContactApp.Models.Contact;

namespace ContactApp.Views;

public partial class AddContactPage : ContentPage
{
	public AddContactPage()
	{
		InitializeComponent();
	}

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        var newContact = new Contact(contactControl.Name, contactControl.Phone, contactControl.Email);
        ContactRepository.AddContact(newContact);
        DisplayAlertAsync("Success", "Contact added successfully.", "OK");
        Shell.Current.GoToAsync("..");
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