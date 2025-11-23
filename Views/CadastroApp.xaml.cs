using MauiAppProva2.Models;
using MauiAppProva2.db;

namespace MauiAppProva2.Views;

public partial class CadastroApp : ContentPage
{
    public CadastroApp()
    {
        InitializeComponent();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
 
        if (string.IsNullOrEmpty(txtNome.Text) ||
            string.IsNullOrEmpty(txtEmail.Text) ||
            string.IsNullOrEmpty(txtSenha.Text))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos!", "OK");
            return; 
        }

        Usuario novoUsuario = new Usuario
        {
            Nome = txtNome.Text,
            DataNascimento = dtpDataNascimento.Date,
            Email = txtEmail.Text,
            Senha = txtSenha.Text
        };

        try
        {
            Db banco = new Db();
            await banco.SalvarUsuarioAsync(novoUsuario);

            await DisplayAlert("Sucesso", "Conta criada com sucesso!", "OK");


            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", "Erro ao salvar: " + ex.Message, "OK");
        }
    }
}