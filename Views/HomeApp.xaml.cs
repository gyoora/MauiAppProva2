using System.Globalization;
using MauiAppProva2.Models;
using MauiAppProva2.Services;
using MauiAppProva2.db;

namespace MauiAppProva2.Views;

public partial class HomeApp : ContentPage
{
    private Usuario _usuarioLogado;

    public HomeApp(Usuario usuario)
    {
        InitializeComponent();

        _usuarioLogado = usuario;
        lblBoasVindas.Text = $"Olá, {usuario.Nome}!";

        dtpInicio.Date = DateTime.Now;
        dtpFim.Date = DateTime.Now;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AtualizarHistorico();
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

                    string tempFormatada = "";
                    if (t.temp_max.HasValue)
                        tempFormatada = $"{t.temp_max.Value.ToString("F0")}°C";
                    else
                        tempFormatada = "--°C";

                    lblTemperatura.Text = tempFormatada;
                    lblDescricao.Text = t.description;
                    lblVento.Text = $"{t.speed} km/h";

                    string mapa = $"https://embed.windy.com/embed.html?" +
                                  $"type=map&location=coordinates&metricRain=mm&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&" +
                                  $"level=surface&lat={t.lat.Value.ToString(CultureInfo.InvariantCulture)}&lon={t.lon.Value.ToString(CultureInfo.InvariantCulture)}";

                    mapaWebView.Source = mapa;

                    Historico novoItem = new Historico
                    {
                        IdUsuario = _usuarioLogado.Id,
                        Cidade = txtCidade.Text.ToUpper(),
                        DataConsulta = DateTime.Now,
                        Temperatura = tempFormatada,
                        Descricao = t.description
                    };

                    Db banco = new Db();
                    await banco.AdicionarHistorico(novoItem);

                    await AtualizarHistorico();
                }
                else
                {
                    lblDescricao.Text = "Sem dados";
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
                lblCoordenadasLocationGPS.Text = $"Lat: {local.Latitude:F4} | Lon: {local.Longitude:F4}";

                await GetCidade(local.Latitude, local.Longitude);
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível obter a localização.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro GPS", ex.Message, "OK");
        }
    }

    private async Task GetCidade(double latitude, double longitude)
    {
        try
        {
            IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
            Placemark? place = places.FirstOrDefault();

            if (place != null)
            {
                txtCidade.Text = place.Locality ?? place.AdminArea ?? "Local Desconhecido";

                OnBuscarClicked(this, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Não conseguimos achar o nome da cidade: " + ex.Message, "OK");
        }
    }

    private async Task AtualizarHistorico()
    {
        try
        {
            Db banco = new Db();
            var lista = await banco.GetHistorico(_usuarioLogado.Id);
            listaHistorico.ItemsSource = lista;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Erro lista: " + ex.Message);
        }
    }

    private async void OnFiltrarClicked(object sender, EventArgs e)
    {
        try
        {
            DateTime dataInicio = dtpInicio.Date;
            DateTime dataFim = dtpFim.Date;

            string cidadeFiltro = txtFiltroCidade.Text.Trim();

            if (dataInicio > dataFim)
            {
                await DisplayAlert("Ops", "A data inicial deve ser menor que a final!", "OK");
                return;
            }

            Db banco = new Db();
            var listaFiltrada = await banco.GetHistoricoFiltrado(_usuarioLogado.Id, dataInicio, dataFim, cidadeFiltro);

            listaHistorico.ItemsSource = listaFiltrada;

            if (listaFiltrada.Count == 0)
            {
                await DisplayAlert("Aviso", "Nenhum registro encontrado.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao filtrar: " + ex.Message, "OK");
        }
    }

    private async void OnLimparFiltroClicked(object sender, EventArgs e)
    {
        txtFiltroCidade.Text = string.Empty;
        dtpInicio.Date = DateTime.Now;
        dtpFim.Date = DateTime.Now;

        await AtualizarHistorico();
    }
}