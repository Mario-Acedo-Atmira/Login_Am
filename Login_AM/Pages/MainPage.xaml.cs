﻿using Login_AM.Data;
using Login_AM.Models;
using Login_AM.Pages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Login_AM.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
        txt_pass.IsPassword = true;
    }
    private async void Loguearse(object sender, EventArgs e)
    {

        if (emailValidator.IsNotValid)
        {
                DisplayAlert("Error", emailValidator.Errors[0].ToString(), "OK");
        }
        else if (passValidator.IsNotValid)
        {
            DisplayAlert("Error", "Tienes que introducir la contraseña", "OK");
        }
        else if(!IsValidPassword(txt_pass.Text))
        {
            DisplayAlert("Error", "La contraseña debe tener 8 carácteres, una mayúscula, una minúscula y un numero", "OK");
        }
        else
        {
            await Login_User(txt_email.Text, txt_pass.Text);
        }
    }
    public async Task Login_User(string username, string password)
    {
        try
        {
            ServicioCertificacion handler = new ServicioCertificacion();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
            string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7083" : "https://localhost:7083";
            string todoUrl = $"{BaseAddress}/api/";
            var userParams = new User(username, password);
            var result = await GetUserParametersAsync(userParams, todoUrl);
            var resultparse = JsonSerializer.Deserialize<UserResponse>(result);
            if (resultparse.token!=null)
            {
                await Navigation.PushAsync(new MenuPage(resultparse.token,resultparse.firstName,resultparse.email));
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private async Task<String> GetUserParametersAsync(User userParams, string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        var response = await client.PostAsJsonAsync(uri + "Auth/login", userParams);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error al realizar la solicitud: {response.StatusCode}");
        }
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

    private async void Registro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroPage());
    }
}
