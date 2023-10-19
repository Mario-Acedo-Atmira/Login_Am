namespace Login_AM.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(string firstName,string email)
	{
		InitializeComponent();
        lblName.Text = firstName;
        lblEmail.Text = email;
	}
}