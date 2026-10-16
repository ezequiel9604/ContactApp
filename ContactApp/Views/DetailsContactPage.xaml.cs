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

    private async void BtnViewMap_Clicked(object sender, EventArgs e)
    {
        if (_contact != null)
        {

            if (string.IsNullOrEmpty(_contact.Address))
            {
                await DisplayAlertAsync("Error", "Address not available.", "Ok");
                return;
            }

            if (_contact.Coordinates == null)
            {
                Location? location = await _contact.GetCoordinatesAsync();

                if (location == null)
                {
                    await DisplayAlertAsync("Error", "Coordinates not available.", "Ok");
                    return;
                }

                await DisplayAlertAsync("Coordinates", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}", "Ok");
                return;
            }

            // await DisplayAlertAsync("Coordinates", $"Latitude: {_contact.Coordinates?.Latitude}, Longitude: {_contact.Coordinates?.Longitude}", "Ok");

            bool opened = await Map.Default.TryOpenAsync(_contact.Coordinates, new MapLaunchOptions { Name = "Contact" });

            if (!opened)
            {
                await DisplayAlertAsync("Error", "No maps app available.", "OK");
                return;
            }
               
        }
    }
}