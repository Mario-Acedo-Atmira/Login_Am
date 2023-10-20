using Firebase.Auth;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Login_AM.Data;
using Login_AM.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Login_AM.Pages;

public partial class AddWorkItemPage : ContentPage
{
    string Token;
    string _email;
	public AddWorkItemPage(string token,string email)
	{
		InitializeComponent();
        Token = token;
        _email = email;
	}
    public async Task AddWorkItem(string titulo, string descripcion)
    {
        try
        {
            int id=0;
            ServicioCertificacion handler = new ServicioCertificacion();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
            string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7083" : "https://localhost:7083";
            string todoUrl = $"{BaseAddress}/api/";
            var response = await ObtenerIdUser(todoUrl);
            List<UserResponse> lista = JsonSerializer.Deserialize<List<UserResponse>>(response);
            foreach (var item in lista)
            {
                if (_email.Equals(item.email))
                {
                    id = item.id;
                }
            }
            var workitemparms = new WorkItemRegister(0, titulo, descripcion, false, id);
            var result = await RegisterParametersAsync(workitemparms, todoUrl);
            await Navigation.PushAsync(new InicioPage(Token,_email));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errores: {ex.Message}");
        }
    }
    private async Task<String> RegisterParametersAsync(WorkItemRegister userParams, string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.PostAsJsonAsync(uri + "WorkItems", userParams);

        return await response.Content.ReadAsStringAsync();
    }
    private async Task<String> ObtenerIdUser(string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.GetAsync(uri + "Users");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error al realizar la solicitud: {response.StatusCode}");
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        subirimagen();
    }
    private async void AddWorkItem_Clicked(object sender, EventArgs e)
    {
        await AddWorkItem(txt_titulo.Text, txt_descripcion.Text);
    }
    private async void subirimagen()
    {
        var result = await MediaPicker.PickPhotoAsync();

        if (result != null)
        {
            var stream = await result.OpenReadAsync();
            try
            {

                if (txt_titulo.Text!=null)
                {
                    var result2 = await UpImage(stream, txt_titulo.Text.Replace(" ","") + "/icono.png");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
    }
    public async Task<string> UpImage(Stream stream, string path)
    {

        try
        {

            FirebaseAuthProvider firebaseAuthProvider = new(new FirebaseConfig("AIzaSyAytxMJqGisvGoaCNvuUjXnMxp6fLSsYnc"));
            FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider
            .SignInWithEmailAndPasswordAsync("danielobry@gmail.com", "Admin12345");
            FirebaseStorageTask storage = new FirebaseStorage("museumatmira.appspot.com", new
            FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
                ThrowOnCancel = false
            })
            .Child(path)
            .PutAsync(stream);





           
            return await storage;

        }
        catch (Exception ex)
        {
            return "Error";
        }

    }
    
}