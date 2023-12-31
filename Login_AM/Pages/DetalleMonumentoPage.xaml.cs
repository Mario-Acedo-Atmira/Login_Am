

using Firebase.Auth;
using Firebase.Storage;
using Login_AM.Data;
using Login_AM.Models;
using System.Text.Json;

namespace Login_AM.Pages;

public partial class DetalleMonumentoPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    string Token;
    int _cont;
    WorkItemsResponse Item;
    public DetalleMonumentoPage(WorkItemsResponse item, string token, int cont)
	{
        
		InitializeComponent();

        Token = token;
        Item = item;
        _cont = cont;
        Conexion();
    }

    private async Task Conexion()
    {
        try
        {
            ServicioCertificacion handler = new ServicioCertificacion();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
            string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7083" : "https://localhost:7083";
            //string BaseAddress = "https://localhost:7083";
            string todoUrl = $"{BaseAddress}/api/";
            client.BaseAddress = new Uri(todoUrl);
            var result = await GetWorkItem(todoUrl);
            var workitem = JsonSerializer.Deserialize<WorkItemsResponse>(result);
            Mostrar_Datos(workitem,todoUrl);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


    }

    private async Task<String> GetWorkItem(string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.GetAsync(uri + "WorkItems/"+Item.id.ToString());

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error al realizar la solicitud: {response.StatusCode}");
        }
    }
    private async void Mostrar_Datos(WorkItemsResponse item, string uri)
    {

        
        titulo.Text = item.title;
        description.Text = item.description;
        if (_cont==0)
        {
            var source = await DownloadImage(item.title.Replace(" ","")+"/icono.png");
            img.Source = source;
        }
        
        foreach (var comment in item.comments)
        {
            VerticalStackLayout views = new VerticalStackLayout();
            views.Margin = new Thickness(0, 10, 0, 0);
            Label nombre = new Label();
            Label comentario = new Label();
            nombre.TextColor=Color.FromRgb(0, 0, 0);
            comentario.TextColor = Color.FromRgb(0, 0, 0);
            var result = await ObtenerNombre(comment.userId, uri);
            var user = JsonSerializer.Deserialize<UserResponse>(result);
            nombre.Text = user.firstName + " " +user.lastName1;
            comentario.Text = comment.comment;
            views.Children.Add(nombre);
            views.Children.Add(comentario);
            Layoutcomentarios.Children.Add(views);
        }

    }
    private async Task<String> ObtenerNombre(int id,string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.GetAsync(uri + "Users/"+id);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error al realizar la solicitud: {response.StatusCode}");
        }
    }

    public async Task<string> DownloadImage(string path)
    {

        try
        {

            FirebaseAuthProvider firebaseAuthProvider = new(new FirebaseConfig("AIzaSyAytxMJqGisvGoaCNvuUjXnMxp6fLSsYnc"));
            FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider
            .SignInWithEmailAndPasswordAsync("danielobry@gmail.com", "Admin12345");
            FirebaseStorageReference storage = new FirebaseStorage("museumatmira.appspot.com", new
            FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
                ThrowOnCancel = false
            })
            .Child(path);

            var storage2 = storage.GetDownloadUrlAsync();
            return await storage2;
        }
        catch (Exception ex)
        {
            return "error";
        }

    }
}