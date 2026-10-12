namespace ContactApp.Views;


public class Contact
{
	public string Initials { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }


    public Contact(string initials, string name, string phone)
	{
		this.Initials = initials;
		this.Name = name;
		this.PhoneNumber = phone;
	}
}

public partial class ContactPage : ContentPage
{
	public ContactPage()
	{
		InitializeComponent();


		List<Contact> contacts = new List<Contact> 
		{ 
			new("JD", "John Doe", "809-111-2222"),
            new("SA", "Samantha Adens", "809-111-2222"),
            new("ML", "Maria Lopez", "809-111-2222")
        };

		listContacts.ItemsSource = contacts;
	}

    public void listContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

		DisplayAlertAsync("Selected Item", "You clicked!",  "Cancel");

    }
}