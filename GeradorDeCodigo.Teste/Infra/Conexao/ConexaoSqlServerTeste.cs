using System.Data.SqlClient;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;
using MbUnit.Framework;


namespace GeradorDeCodigo.Teste.Infra.Conexao
{
    [TestFixture]
    public class ConexaoSqlServerTeste
    {
        [Test]
        public void Passando_source_e_base_ele_retrona_uma_conexao()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);
            StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS", "Suppin");

            Assert.IsNotNull(conexao.Connection<SqlConnection>());

            StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS");

            Assert.IsNotNull(conexao.Connection<SqlConnection>());

        }

 
        [Test]
        public void Retorana_verdadeiro_se_a_conexao_estiver_aberta()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);
            StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS", "Suppin");
            var con = conexao.Connection<SqlConnection>();
            Assert.IsTrue(conexao.TesteConexao());

        }

        [Test]
        public void Retorana_falso_se_a_conexao_nao_estiver_aberta()
        {

            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);
            StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS", "Suppin");
            conexao.Dispose();
            Assert.IsFalse(conexao.TesteConexao());

        }


        [Test]
        public void nao_passando_source_e_base_ele_retorna_uma_null()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);
            StringConexao.GetSqlServer(@"VGA-DESSQLEXPRESS", "Suppin");

            Assert.IsNull(conexao.Connection<SqlConnection>());

        }
    
  

    }
}
