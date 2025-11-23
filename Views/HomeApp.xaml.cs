using MauiAppProva2.Models;   
using MauiAppProva2.Services;

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

                    lblSol.Text = $"{t.sunrise} | {t.sunset}";


                    string max = t.temp_max.HasValue ? t.temp_max.ToString() : "?";
                    string min = t.temp_min.HasValue ? t.temp_min.ToString() : "?";

                    lblTemperatura.Text = $"{max}° / {min}°";

                    lblDescricao.Text = t.description;

                    lblVento.Text = $"{t.speed} km/h";
                }
                else
                {

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

    private async void LocalizacaoClicked(object sender, EventArgs e)
    {
        try
        {
            GeolocationRequest request = new GeolocationRequest(
                GeolocationAccuracy.Medium, 
                TimeSpan.FromSeconds(10)
            );

            Location? local = await Geolocation.Default.GetLocationAsync(request);

            if (local != null)
            {
                string local_disp = $"Latitude: {local.Latitude}, Longitude: {local.Longitude}";

                lblCoordenadasLocationGPS.Text = local_disp;
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível obter a localização.", "OK");
            }

        }
        catch (FeatureNotSupportedException fnsEx)
        {
            await DisplayAlert("Erro: Dispositivo não suporta", fnsEx.Message, "OK");
        }
        catch (FeatureNotEnabledException fneEx)
        {
            await DisplayAlert("Erro: Localização desabilitada", fneEx.Message, "OK");
        }
        catch (PermissionException pEx)
        {
            await DisplayAlert("Erro: Permissão da localização", pEx.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}