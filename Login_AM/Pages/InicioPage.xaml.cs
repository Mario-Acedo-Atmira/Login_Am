using Login_AM.Data;
using Login_AM.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Login_AM.Pages;

public partial class InicioPage : ContentPage
{
    string Token;
    public InicioPage(string token)
	{
		InitializeComponent();
        Token = token;
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
            var result = await GetWorkItems(todoUrl);
            var lista = JsonSerializer.Deserialize<List<WorkItemsResponse>>(result);
            Mostrar_Datos(lista);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        

    }
    private async Task<String> GetWorkItems(string uri)
    {
        ServicioCertificacion handler = new ServicioCertificacion();
        HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.GetAsync(uri + "WorkItems");
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error al realizar la solicitud: {response.StatusCode}");
        }
    }
    private async void Mostrar_Datos(List<WorkItemsResponse> lista)
    {
        foreach (var item in lista) 
        {
            if (item.isAThought == false)
            {
               
                
                VerticalStackLayout ChildLayout = new VerticalStackLayout();
                ImageButton btn_image = new ImageButton();
                Label label = new Label();
                btn_image.Source = "Monumento3.png";
                btn_image.HorizontalOptions = LayoutOptions.Center;
                btn_image.VerticalOptions = LayoutOptions.Center;
                btn_image.WidthRequest = 360;
                btn_image.HeightRequest = 360;
                btn_image.Margin = new Thickness(10, 10, 0, 0);

                label.Text = item.title;
                label.TextColor = Color.FromRgb(0, 0, 0);
                label.WidthRequest = 360;
                label.HorizontalOptions = LayoutOptions.Center;
                label.Margin = new Thickness(10, -1, 0, 0);
                ChildLayout.Children.Add(btn_image);
                ChildLayout.Children.Add(label);
                layout.Children.Add(ChildLayout);
                btn_image.Clicked += delegate (object sender, EventArgs e)
                {
                    Navigation.PushAsync(new DetalleMonumentoPage(item,Token));
                };
            }
            
        }
        
        
        
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddWorkItemPage());
    }
}