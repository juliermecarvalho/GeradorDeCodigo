using System;


namespace GeradorDeCodigo.Infra.Conexao
{
    public class ConexaoOracle: IConexao
    {

        private static ConexaoOracle _conexaoOracle;

        private ConexaoOracle()
        {            
        }

        public static IConexao GetInstace()
        {
            if (_conexaoOracle == null)
            {
                _conexaoOracle = new ConexaoOracle();
            }
            return _conexaoOracle;
        }

        public T Connection<T>()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool TesteConexao()
        {
            throw new NotImplementedException();
        }

      
    }
}
