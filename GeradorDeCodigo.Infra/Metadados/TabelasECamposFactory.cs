using GeradorDeCodigo.Infra.Enun;

namespace GeradorDeCodigo.Infra.Metadados
{
    public class TabelasECamposFactory
    {

        private TabelasECamposFactory()
        {
        }

        public static ITabelasECampos GetTabelasECampos(TipoDeBancoDados tipoDeBancoDados)
        {
            if(tipoDeBancoDados == TipoDeBancoDados.Sqlserver)
            {
                return new TabelasECamposSqlServer();
            }

            return null;
        }


    }
}
