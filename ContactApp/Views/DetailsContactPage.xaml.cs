using ContactApp.Models;
using Contact = ContactApp.Models.Contact;

namespace ContactApp.Views;

[QueryProperty(nameof(ContactId), "Id")]
public partial class DetailsContactPage : ContentPage
{

	private Contact? _contact;

    public string ContactId
    {
        set
        {
            _contact = ContactRepository.GetContactById(Convert.ToInt32(value));
            BindingContext = _contact;
        }
    }

    public DetailsContactPage()
	{
		InitializeComponent();
	}

    public async void BtnViewMap_Clicked(object sender, EventArgs e)
    {
        if (_contact != null)
        {
            if (_contact.Coordinates == null)
            {
                await DisplayAlertAsync("Error", "Coordinates not available.", "Ok");
                return;
            }
            
            Console.WriteLine($"Coordinates: Latitude={_contact.Coordinates?.Latitude}, Longitude={_contact.Coordinates?.Longitude}");
        }
    }
}