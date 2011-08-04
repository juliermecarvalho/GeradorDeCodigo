using System;

namespace GeradorDeCodigo.Infra.Conexao
{
    public interface IConexao: IDisposable
    {

        
        /// <summary>
        /// Reponsável por retorna um conexão.
        /// </summary>
        T Connection<T>(); 
        /// <summary>
        /// Testa a conexão esta aberta ou fechada,
        /// retornado verdadeiro se a mesma estiver aberta.
        /// </summary>
        /// <returns></returns>
        bool TesteConexao();
    }

   
}
