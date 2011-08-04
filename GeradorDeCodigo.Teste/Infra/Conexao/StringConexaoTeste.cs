using System;
using MbUnit.Framework;


namespace GeradorDeCodigo.Infra.Conexao
{
    [TestFixture]
    public class StringConexaoTeste
    {
        [Test]
        public void Passado_so_Souce_Tem_que_retorna_string_conexao()
        {
            const string source = "sa";
            StringConexao.GetSqlServer(source);
            Assert.AreEqual(@"Data Source=" + source + ";Integrated Security=True",
                StringConexao.Stringconexao);
        }

        [Test]
        public void Passado_so_Souce_Catalog_Tem_que_retorna_string_conexao()
        {
            const string source = "sa";
            const string catalog = "suppin";
            StringConexao.GetSqlServer(source, catalog);
            Assert.AreEqual(@"Data Source=" + source + ";Initial Catalog=" + catalog + ";Integrated Security=True",
                StringConexao.Stringconexao);
        }


        [Test]
        public void Passado_so_Souce_ID_Passaword_Tem_que_retorna_string_conexao()
        {
            const string host = "sa";
            const string id = "suppin";
            const string password = "suppin";
            const string porta = "port";
            const string servename = "servename";
            StringConexao.GetOralce(host, porta, servename, id, password);
            Assert.AreEqual(@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + porta + ")))(CONNECT_DATA=(SERVICE_NAME=" + servename + ")));User ID=" + id + ";Password=" + password,
                StringConexao.Stringconexao);
        }

        [Test]
        public void Passado_so_Souce_Catalog_ID_Passaword_Tem_que_retorna_string_conexao()
        {
            const string source = "sa";
            const string catalog = "suppin";
            const string id = "suppin";
            const string password = "suppin";
            StringConexao.GetSqlServer(source, catalog, id, password);
            Assert.AreEqual(@"Data Source=" + source + ";Initial Catalog=" + catalog + ";User ID=" + id + ";Password=" + password,
                StringConexao.Stringconexao);
        }

        [Test]
        public void Retorna_uma_exception_GetSqlServer_se_passa_source_vazio()
        {
            
            Assert.Throws<Exception>(() => StringConexao.GetSqlServer(""));
        }

        [Test]
        public void Retorna_uma_exception_GetSqlServer_se_passa_source_catalog_vazio()
        {

            Assert.Throws<Exception>(() => StringConexao.GetSqlServer("", ""));

        }

        [Test]
        public void Retorna_uma_exception_GetSqlServer_se_passa_source_id_password_vazio()
        {


            Assert.Throws<Exception>(() => StringConexao.GetOralce("", "", "", "", ""));

        }

        [Test]
        public void Retorna_uma_exception_GetOracle_se_passa_source_Catalog_id_password_vazio()
        {


            Assert.Throws<Exception>(() => StringConexao.GetSqlServer("", "", "", ""));

        }
    }
}
