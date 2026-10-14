namespace ContactApp.Views.Controls;

public partial class ContactControl : ContentView
{

    public event EventHandler<string> OnError;
    public event EventHandler OnCancelClicked;
    public event EventHandler OnSaveClicked;

    public ContactControl()
	{
		InitializeComponent();
	}

	public string Name
    {
        get => nameEntry.Text;
        set => nameEntry.Text = value;
    }

    public string Phone
    {
        get => phoneEntry.Text;
        set => phoneEntry.Text = value;
    }

    public string Email
    {
        get => emailEntry.Text;
        set => emailEntry.Text = value;
    }

    public void BtnSave_clicked(object sender, EventArgs e)
    {
        if (nameValidation.IsNotValid)
        {
            // DisplayAlertAsync("Error", "Name must be at least 5 characters long.", "OK");
            OnError?.Invoke(this, "Name is required.");
            return;
        }

        if (emailValidation.IsNotValid)
        {

            foreach (var error in emailValidation.Errors)
            {
                OnError?.Invoke(this, error.ToString());
            }
            return;
        }

        OnSaveClicked?.Invoke(this, e);

    }

    public void BtnCancel_clicked(object sender, EventArgs e)
    {
        // Raise the CancelClicked event
        OnCancelClicked?.Invoke(this, e);
    }

}