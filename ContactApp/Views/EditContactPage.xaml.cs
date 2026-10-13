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

    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    public string ContactId
    {
        get { return Contact?.Id.ToString() ?? string.Empty; }
        set
        {
            Contact = ContactRepository.GetContactById(Convert.ToInt32(value));
            if (Contact != null)
            {
                nameEntry.Text = Contact.Name;
                phoneEntry.Text = Contact.PhoneNumber;
                emailEntry.Text = Contact.Email ?? string.Empty;
            }
        }
    }

    public void OnSaveClicked(object sender, EventArgs e)
    {
        if (Contact != null)
        {
            Contact.Name = nameEntry.Text;
            Contact.PhoneNumber = phoneEntry.Text;
            Contact.Email = emailEntry.Text;
            ContactRepository.UpdateContact(Contact);
        }
        Shell.Current.GoToAsync("..");
    }

    public void OnCancelClicked(object sender, EventArgs e)
    {
        if (Contact != null)
        {
           // ContactRepository.DeleteContact(Contact.Id);
        }
        Shell.Current.GoToAsync("..");
    }

    public void OnFieldChanged(object sender, EventArgs e)
    {
        
    }

}