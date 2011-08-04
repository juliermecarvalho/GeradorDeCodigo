using GeradorDeCodigo.Infra.Enun;

namespace GeradorDeCodigo.Infra.Conexao
{
    public class FactoryConexao
    {
        

        public static IConexao GetConexao(TipoDeBancoDados tipoDeBancoDados)
        {
            

            if(tipoDeBancoDados == TipoDeBancoDados.Sqlserver)
            {
                return ConexaoSqlServer.GetInstace();
            }
            if(tipoDeBancoDados == TipoDeBancoDados.Oracle)
            {
                return ConexaoOracle.GetInstace();
            }

            return null;
        }
    }


}
