using AppNomesBr.Domain.DataTransferObject.ExternalIntegrations.IBGE.Censos;
using AppNomesBr.Domain.Interfaces.Services;
using System.Text.Json;

namespace AppNomesBr.Pages;

public partial class RankingNomesBrasileiros : ContentPage
{
    private readonly INomesBrService service;
    public RankingNomesBrasileiros(INomesBrService service)
    {
        InitializeComponent();
        this.service = service;
        BtnAtualizar.Clicked += BtnAtualizar_Clicked;
    }

    protected override async void OnAppearing()
    {
        await CarregarNomes();
        base.OnAppearing();
    }

    private async Task CarregarNomes()
    {
        var result = await service.ListaTop20Nacional();
        this.GrdNomesBr.ItemsSource = result.FirstOrDefault()?.Resultado;
    }

    private async void BtnAtualizar_Clicked(object? sender, EventArgs e)
    {
        await AtualizarNomesComFiltros();
    }

    private async Task AtualizarNomesComFiltros()
    {
        var sexo = SexoMRadioButton.IsChecked ? "M" : SexoFRadioButton.IsChecked ? "F" : "Todos";
        var municipio = MunicipioEntry.Text;

        // Obter o código IBGE para o município inserido
        var codigoIbge = await ObterCodigoIBGEPorMunicipio(municipio);

        if (!string.IsNullOrEmpty(codigoIbge))
        {
            // Se o código IBGE foi encontrado, faça a consulta com base no código
            var result = await service.ListaTop20PorMunicipioESexo(codigoIbge, sexo);
            GrdNomesBr.ItemsSource = result.FirstOrDefault()?.Resultado;
        }
        else
        {
            // Caso o município não tenha sido encontrado, exiba uma mensagem de erro
            await DisplayAlert("Erro", "Município não encontrado!", "OK");
        }
    }
    private async Task<string?> ObterCodigoIBGEPorMunicipio(string nomeMunicipio)
    {
        try
        {
            using HttpClient client = new();
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/municipios/{nomeMunicipio}";
            var response = await client.GetStringAsync(url);

            // Deserializando a resposta JSON
            var municipioResponse = JsonSerializer.Deserialize<MunicipioIbgeResponse>(response);

            // Retorna o código IBGE, que é o campo "id"
            return municipioResponse?.id.ToString();
        }
        catch (Exception ex)
        {
            // Tratar possíveis erros, como município não encontrado ou falha na chamada da API
            Console.WriteLine($"Erro ao obter código IBGE: {ex.Message}");
            return null;
        }
    }

    public class MunicipioIbgeResponse
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}