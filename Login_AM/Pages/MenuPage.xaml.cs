namespace Login_AM.Pages;

public partial class MenuPage : ContentPage
{
    string _token;
    string _firstName;
    string _email;
    public MenuPage(string token,string firstName,string email)
	{
		InitializeComponent();
        _token = token;
        _firstName = firstName;
        _email = email;
        lblBienvenido.Text = "Bienvenido, "+_firstName;
	}

    private void InicioButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new InicioPage(_token, _email));
    }
    private void ForumButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ForumPage(_token));
    }
    private void FavouriteButton_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new InicioPage(_token));
    }
    private void ProfileButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProfilePage(_firstName,_email));
    }
}