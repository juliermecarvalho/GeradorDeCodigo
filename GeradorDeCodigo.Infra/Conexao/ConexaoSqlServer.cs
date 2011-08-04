using System;
using System.Data;
using System.Data.SqlClient;

namespace GeradorDeCodigo.Infra.Conexao
{
    public sealed class ConexaoSqlServer : IConexao
    {
        private static readonly ConexaoSqlServer ConexaoSqlserver = new ConexaoSqlServer();
        private static string _conexaoString;
        private IDbConnection _sqlConnection;

    
        private ConexaoSqlServer()
        {
            
        }

        public static IConexao GetInstace()
        {
            return ConexaoSqlserver;
        }


        public T Connection<T>()
        {
            try
            {
                if (_sqlConnection == null || _conexaoString != StringConexao.Stringconexao)
                {
                    _sqlConnection = new SqlConnection(StringConexao.Stringconexao);
                    _conexaoString = StringConexao.Stringconexao;
                }

                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                return (T)_sqlConnection;
            }
            catch
            {
                return default(T);
            }
        }

        public void Dispose()
        {
            if (_sqlConnection != null)
            {
                if (_sqlConnection.State == ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
                _sqlConnection = null;
            }
            GC.SuppressFinalize(this);
        }

    
        public bool TesteConexao()
        {
            try
            {
                var abrir = Connection<SqlConnection>();
                var toRetrun = _sqlConnection.State == ConnectionState.Open;
                abrir.Dispose();
                return toRetrun;
            }
            catch
            {
                
                return false;
            }
        }


    }
}
