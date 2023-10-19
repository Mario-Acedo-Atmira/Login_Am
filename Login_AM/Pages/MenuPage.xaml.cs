namespace Login_AM.Pages;

public partial class MenuPage : ContentPage
{
    string _token;
    string _firstName;
    public MenuPage(string token,string firstName)
	{
		InitializeComponent();
        _token = token;
        _firstName = firstName;
        lblBienvenido.Text = "Bienvenido, "+_firstName;
	}

    private void InicioButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new InicioPage(_token));
    }
}