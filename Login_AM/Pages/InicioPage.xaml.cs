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
    private async void DetalleIdea2(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new DetalleMonumentoPage("monumento2.png", "Monumento 2"));
    }
    private async void DetalleIdea3(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new DetalleMonumentoPage("monumento3.png", "Monumento 3"));
    }
    private async void DetalleIdea4(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new DetalleMonumentoPage("monumento4.png", "Monumento 4"));
    }
}