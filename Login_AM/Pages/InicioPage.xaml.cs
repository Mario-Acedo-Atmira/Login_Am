namespace Login_AM.Pages;

public partial class InicioPage : ContentPage
{
	public InicioPage(string token)
	{
		InitializeComponent();
	}
    private async void DetalleIdea1(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new DetalleMonumentoPage("monumento1.png", "Monumento 1"));
    }
    
}