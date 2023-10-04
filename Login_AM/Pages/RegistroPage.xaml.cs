using Login_AM.Models;
using Login_AM.Data;
using Login_AM.Pages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Login_AM.Pages;

public partial class RegistroPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    public RegistroPage()
	{
		InitializeComponent();
        BindingContext = this;
        txt_pass.IsPassword = true;
    }

    private async void Registrarse(object sender, EventArgs e)
    {
        Entry txt_nombre = (Entry)FindByName("txt_nombre");
        Entry txt_ape1 = (Entry)FindByName("txt_ape1");
        Entry txt_ape2 = (Entry)FindByName("txt_ape2");
        Entry txt_tel = (Entry)FindByName("txt_tel");
        Entry txt_email = (Entry)FindByName("txt_email");
        Entry txt_pass = (Entry)FindByName("txt_pass");
        await Login_User(txt_email.Text, txt_pass.Text, txt_nombre.Text, txt_ape1.Text, txt_ape2.Text, txt_tel.Text);
    }
    public async Task Login_User(string username, string password, string nombre, string ape1, string ape2, string tel)
    {
        try
        {
            //ServicioCertificacion handler = new ServicioCertificacion();
            //HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
            //string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7083" : "https://localhost:7083";
            string BaseAddress = "https://localhost:7083";
            string todoUrl = $"{BaseAddress}/api/";
            _httpClient.BaseAddress = new Uri(todoUrl);
            var userParams = new UserRegister(0,nombre,ape1,ape2,tel, username, password);
            var result = await RegisterParametersAsync(userParams, todoUrl);
            await Navigation.PushAsync(new MainPage());

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errores: {ex.Message}");
        }
    }
    private async Task<String> RegisterParametersAsync(UserRegister userParams, string uri)
    {
        //ServicioCertificacion handler = new ServicioCertificacion();
        //HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());

        var response = await _httpClient.PostAsJsonAsync(uri + "Auth/register", userParams);

            return await response.Content.ReadAsStringAsync();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = sender as ImageButton;
        if (txt_pass.IsPassword)
        {
            imageButton.Source = ImageSource.FromFile("eyeoff.png");
            txt_pass.IsPassword = false;
        }
        else
        {
            imageButton.Source = ImageSource.FromFile("eyeon.png");
            txt_pass.IsPassword = true;
        }
    }
}