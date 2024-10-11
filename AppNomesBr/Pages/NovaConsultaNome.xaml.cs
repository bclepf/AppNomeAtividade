using AppNomesBr.Domain.Interfaces.Repositories;
using AppNomesBr.Domain.Interfaces.Services;

namespace AppNomesBr.Pages;

public partial class NovaConsultaNome : ContentPage
{
    private readonly INomesBrService service;
    private readonly INomesBrRepository repository;

    public NovaConsultaNome(INomesBrService service, INomesBrRepository repository)
    {
        InitializeComponent();
        this.service = service;
        this.repository = repository;
        BtnPesquisar.Clicked += BtnPesquisar_Clicked;
        BtnDeleteAll.Clicked += BtnDeleteAll_Clicked;
        SexOpt();
    }

    private async void BtnDeleteAll_Clicked(object? sender, EventArgs e)
    {
        var registros = await repository.GetAll();

        foreach (var registro in registros)
            await repository.Delete(registro.Id);

        await CarregarNomes();
    }

    protected override async void OnAppearing()
    {
        await CarregarNomes();
        base.OnAppearing();
    }

    private async void BtnPesquisar_Clicked(object? sender, EventArgs e)
    {
        if (PKSX.SelectedItem == null)
        {
            await DisplayAlert("Erro", "Por favor, selecione um sexo", "OK");
            return;
        }
        var sexoSelcionado = PKSX.SelectedItem?.ToString() ?? string.Empty;
        await service.InserirNovoRegistroNoRanking(TxtNome.Text.ToUpper(), sexoSelcionado);
        await CarregarNomes();
       
    }

    private async Task CarregarNomes()
    {
        var result = await service.ListaMeuRanking();
        this.GrdNomesBr.ItemsSource = result.FirstOrDefault()?.Resultado;
    }

    private void SexOpt()
    {
        var sex = new List<string>
        {
        "M", "F"
        };
        PKSX.ItemsSource = sex;
    }
    private async void PickerSexoFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sexoSelecionado = PickerSexoFiltro.SelectedItem as string;

        if (sexoSelecionado == "Todas")
        {
            await CarregarNomes(); // Carrega todos os nomes
        }
        else
        {
            await CarregarNomesPorSexo(sexoSelecionado); // Carrega os nomes filtrados por sexo
        }
    }

    private async Task CarregarNomesPorSexo(string sexo)
    {
        var result = await service.ListaMeuRanking();

        // Filtra os nomes de acordo com o sexo selecionado
        var nomesFiltrados = result.FirstOrDefault()?.Resultado
            .Where(n => n.Sexo == sexo)
            .ToList();

        this.GrdNomesBr.ItemsSource = nomesFiltrados;
    }

    private async void BtnRemoverFiltro_Clicked(object sender, EventArgs e)
    {
        PickerSexoFiltro.SelectedItem = "Todas"; // Reseta o Picker para "Todas"
        await CarregarNomes(); // Carrega todos os nomes
    }

}