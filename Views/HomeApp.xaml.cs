using MauiAppProva2.Models;

namespace MauiAppProva2.Views;

public partial class HomeApp : ContentPage
{
    public HomeApp(string nomeUsuario)
    {
        InitializeComponent();
        lblBoasVindas.Text = $"Olá, {nomeUsuario}!";
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {

    }
}