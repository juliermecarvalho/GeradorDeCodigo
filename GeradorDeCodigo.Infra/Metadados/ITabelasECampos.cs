using System.Collections.Generic;

namespace GeradorDeCodigo.Infra.Metadados
{
    public interface ITabelasECampos
    {
        /// <summary>
        /// Verifica se existe a tabela no banco de dados
        /// </summary>
        /// <param name="tabela"></param>
        /// <returns>true ou false</returns>
        bool VerificaSeExisteTabela(string tabela);
        /// <summary>
        /// Retorna o tamanho de que campos string
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Campo da tabela</param>
        /// <returns></returns>
        string TamanhodoCamposString(string tabela, string campo);
        /// <summary>
        /// Retorna todas as tabelas do banco de dados
        /// de acordo com a conexão.
        /// </summary>
        /// <returns>Todas as tabelas</returns>
        IEnumerable<string> TabelasDoBanco();
        /// <summary>
        /// Retorna todos os campos da tabela passada como parâmetro.
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <returns>Todos os campos da tabela</returns>
        IEnumerable<string> CamposDaTabela(string tabela);
        /// <summary>
        /// Retorna todos os campos chave da tabela passada com parâmetro.
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <returns>Retorna todos os campos chave</returns>
        IEnumerable<string> CamposChavesDaTabela(string tabela);
        /// <summary>
        /// Retorna verdadeiro se um campo de uma determinada tabela for
        /// campo chave.
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Campo da tabela</param>
        /// <returns>Verdadeiro ou falso</returns>
        bool CampoChavesDaTabela(string tabela, string campo);
        /// <summary>
        /// Retorna o dialeto de acordo com a versão do banco de dados
        /// </summary>
        /// <returns>retorna o dialeto.</returns>
        string DialectDoBancodeDados();
        /// <summary>
        /// Retorna o Driver a ser usando pelo NHibernate.
        /// </summary>
        /// <returns></returns>
        string ConnectionDriver();
        /// <summary>
        /// Retorna um campos chave da tabela
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <returns>Nome do campo da tabela que chave primaria</returns>
        string CampoChavesDaTabela(string tabela);
        /// <summary>
        /// Retorna se um campos é obrigatório ou não.
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Nome do campo na tabela</param>
        /// <returns>true ou false</returns>
        bool CampoOBrigatorio(string tabela, string campo);
        /// <summary>
        /// Retorna um lista de nome de tabelas onde há relacionamento
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <returns>Lista com nome de tabelas onde há relacionamento</returns>
        IEnumerable<string> TabelasOndeARelacionamento(string tabela);
        /// <summary>
        /// Retorna os campos repetido de um relacionamento
        /// </summary>
        /// <param name="tabela"></param>
        /// <returns></returns>
        IEnumerable<string> TabelasOndeARelacionamentoCamposRepetidos(string tabela);
        /// <summary>
        /// Passado uma tabela e campo e retorna se campo Identity da abela
        /// </summary>
        /// <param name="tabela">Nome da Tabela</param>
        /// <param name="campo">Campo da Tabela</param>
        /// <returns>verdadeiro ou falso</returns>
        bool CamposIdentityDaTabela(string tabela, string campo);
        /// <summary>
        /// Retorna uma lista com os campos que são chaves estrágeiras da tabela
        /// </summary>
        /// <param name="tabela">Nome da Tabela</param>
        /// <returns>Lista de com os campos que são chave estrágeiras</returns>
        IEnumerable<string> CamposChavesEstrageiraDaTabela(string tabela);
        /// <summary>
        /// Retorna o nome da tabela mãe de uma chave estrágeira
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Nome do campo na tabela</param>
        /// <returns>Retorna o nome da tabela</returns>
        string NomeDaTabelaDaChaveEstrgeira(string tabela, string campo);
        /// <summary>
        /// Retorna o tipo no banco de dados
        /// </summary>
        /// <param name="tabela">Nome da Tabela</param>
        /// <param name="campo">Nome do Campo na Tabela</param>
        /// <returns>Retorna o tipo que ele represento no banco de dados</returns>
        string TipoDadoBd(string tabela, string campo);
        /// <summary>
        /// Retorna o nome do campo onde a relacionamento.
        /// </summary>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="fkTabela">Nome da tabela onde ela é chave estangeira</param>
        /// <returns>Retorna o nome do campo da chave estangeira</returns>
        string NomeDoCampoOndeARelacionmento(string tabela, string fkTabela);

    }
}
