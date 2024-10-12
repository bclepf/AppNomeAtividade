namespace AppNomesBr.Domain.Interfaces.ExternalIntegrations.IBGE.Censos
{
    public interface INomesApi
    {
        Task<string> RetornaCensosNomesRanking();
        Task<string> RetornaCensosNomesRankingFiltros(string MunicipioEntry, string SexoOpc);
        Task<string> RetornaCensosNomesPeriodo(string nome);
        
    }
}
