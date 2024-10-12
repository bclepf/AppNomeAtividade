using AppNomesBr.Domain.DataTransferObject.ExternalIntegrations.IBGE.Censos;

namespace AppNomesBr.Domain.Interfaces.Services
{
    public interface INomesBrService
    {
        Task<RankingNomesRoot[]> ListaTop20Nacional();
        Task<RankingNomesRoot[]> ListaMeuRanking();
        Task<RankingNomesRoot[]> ListaTop20PorMunicipioESexo(string municipio, string sexo);
        Task InserirNovoRegistroNoRanking(string codigoIbge, string sexo);
    }
}
