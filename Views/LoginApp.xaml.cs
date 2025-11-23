namespace MauiAppProva2.Views;

public partial class LoginApp : ContentPage
{
	public LoginApp()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CadastroApp());
    }

    private async void OnEntrarClicked(object sender, EventArgs e)
    {
        string email = txtEmailLogin.Text;
        string senha = txtSenhaLogin.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            await DisplayAlert("Erro", "Preencha email e senha!", "OK");
            return;
        }

        try
        {
            var dataBase = new MauiAppProva2.db.Db();

            var usuarioExiste = await dataBase.GetUsuarioAsync(email, senha);

            if (usuarioExiste != null)
            {
                Application.Current.MainPage = new NavigationPage(new HomeApp());
            }
            else
            {
                await DisplayAlert("Ops!", "Email ou senha incorretos.", "OK");
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }
}   