using ContactApp.Models;
using Contact = ContactApp.Models.Contact;

namespace ContactApp.Views;

[QueryProperty(nameof(ContactId), "Id")]
public partial class EditContactPage : ContentPage
{

    private Contact? Contact;

    public EditContactPage()
    {
        InitializeComponent();
    }

    public string ContactId
    {
        get { return Contact?.Id.ToString() ?? string.Empty; }
        set
        {
            Contact = ContactRepository.GetContactById(Convert.ToInt32(value));
            if (Contact != null)
            {
                contactControl.Name = Contact.Name;
                contactControl.Phone = Contact.PhoneNumber;
                contactControl.Email = Contact.Email ?? string.Empty;
            }
        }
    }

    public void BtnUpdate_Clicked(object sender, EventArgs e)
    {
        if (Contact != null)
        {
            Contact.Name = contactControl.Name;
            Contact.PhoneNumber = contactControl.Phone;
            Contact.Email = contactControl.Email;
            ContactRepository.UpdateContact(Contact);

            DisplayAlertAsync("Success", "Contact updated successfully.", "OK");
        }
        Shell.Current.GoToAsync("..");
    }

    public void BtnCancel_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    public void ContactControl_OnError(object sender, string errorMessage)
    {
        DisplayAlertAsync("Error", errorMessage, "OK");
    }

}