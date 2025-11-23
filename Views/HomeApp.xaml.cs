using MauiAppProva2.Services;
using MauiAppProva2.Models;   

namespace MauiAppProva2.Views;

public partial class HomeApp : ContentPage
{
    public HomeApp(string nomeUsuario)
    {
        InitializeComponent();
        lblBoasVindas.Text = $"Olá, {nomeUsuario}!";
    }

    private async void OnBuscarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCidade.Text))
            {
                
                Tempo? t = await DataService.GetPrevisao(txtCidade.Text);

                if (t != null)
                {

                    lblCidade.Text = txtCidade.Text.ToUpper();

                    
                    lblCoordenadas.Text = $"Lat: {t.lat} | Lon: {t.lon}";

                    // 3. Nascer e Pôr do Sol (Dados do prof)
                    lblSol.Text = $"{t.sunrise} | {t.sunset}";

                    // 4. Temperaturas Máx e Mín (Dados do prof)
                    // Se algum vier nulo, mostra "?", senão mostra o número
                    string max = t.temp_max.HasValue ? t.temp_max.ToString() : "?";
                    string min = t.temp_min.HasValue ? t.temp_min.ToString() : "?";

                    lblTemperatura.Text = $"{max}° / {min}°";

                    // 5. Descrição (Extra para ficar bonito)
                    lblDescricao.Text = t.description;

                    // 6. Vento (Extra para preencher o espaço)
                    lblVento.Text = $"{t.speed} km/h";
                }
                else
                {
                    // Caso não encontre dados
                    lblDescricao.Text = "Sem dados de Previsão";
                    await DisplayAlert("Ops", "Cidade não encontrada", "OK");
                }
            }
            else
            {
                await DisplayAlert("Atenção", "Preencha a cidade.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}