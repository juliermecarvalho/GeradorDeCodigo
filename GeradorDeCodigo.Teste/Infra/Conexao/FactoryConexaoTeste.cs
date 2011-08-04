using System;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;
using MbUnit.Framework;

namespace GeradorDeCodigo.Teste.Infra.Conexao
{
    [TestFixture]
    public class FactoryConexaoTeste
    {
       

        [Test]
        public void Retorna_uma_class_ConexaoSqlServer()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);

            Assert.IsNotNull(conexao);
        }

       [Test]
        public void Retorna_uma_class_ConexaoOracle()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Oracle);

            Assert.IsNotNull(conexao);
        }


       [Test]
        public void Retorna_uma_class_tipo_ConexaoOracle()
        {
            
            IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Oracle);

            Assert.IsInstanceOfType<ConexaoOracle>(conexao);
           
        }

       [Test]
       public void Retorna_uma_class_tipo_ConexaoSqlServe()
       {
           
           IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Sqlserver);

           Assert.IsInstanceOfType<ConexaoSqlServer>(conexao);

       }

       [Test]
       public void Retorna_uma_exeption_caso_passe_Oracle_e_retorna_Sqlserver()
       {
           
           IConexao conexao = FactoryConexao.GetConexao(TipoDeBancoDados.Oracle);

           Assert.Throws<Exception>(() => Assert.IsInstanceOfType<ConexaoSqlServer>(conexao));

       }
    

    }

   
}
