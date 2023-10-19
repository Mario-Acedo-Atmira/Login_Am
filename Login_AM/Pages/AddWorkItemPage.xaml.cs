using Firebase.Auth;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
namespace Login_AM.Pages;

public partial class AddWorkItemPage : ContentPage
{
	public AddWorkItemPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        subirimagen();
        
        var result2 = await DownloadImage(txt_titulo.Text.Replace(" ", ""));
        imagen.Source= result2;
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
                    var result2 = await UpImage(stream, txt_titulo.Text.Replace(" ","") + "/icono");
                    Thread.Sleep(5000);
                }
                



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //await UploadImageToFirebaseStorage(stream,"MalenoKebab/malenokebab.jpg");
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
            return "Error";
        }

    }
}