using Login_AM.Models;
using Login_AM.Data;
using Login_AM.Pages;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Login_AM.Pages;

public partial class RegistroPage : ContentPage
{
	public RegistroPage()
	{
		InitializeComponent();
        BindingContext = this;
        txt_pass.IsPassword = true;
    }

    public async void Registrarse(object sender, EventArgs e)
    {
        if (nameValidator.IsNotValid)
        {
            DisplayAlert("Error", "Tienes que introducir el nombre", "OK");
        }
        else
        {
            await Login_User(txt_email.Text, txt_pass.Text, txt_nombre.Text, txt_ape1.Text, txt_ape2.Text, txt_tel.Text);
        }
    }
    public async Task Login_User(string username, string password, string nombre, string ape1, string ape2, string tel)
    {
        try
        {
            ServicioCertificacion handler = new ServicioCertificacion();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
            string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7083" : "https://localhost:7083";
            string todoUrl = $"{BaseAddress}/api/";
            client.BaseAddress = new Uri(todoUrl);
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
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        var response = await client.PostAsJsonAsync(uri + "Auth/register", userParams);

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

    private bool IsValidPassword(string password)
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasMinimum8Chars = new Regex(@".{8,}");

        var isValid = hasNumber.IsMatch(password) &&
            hasUpperChar.IsMatch(password) &&
            hasLowerChar.IsMatch(password) &&
            hasMinimum8Chars.IsMatch(password);

        return isValid;
    }
}