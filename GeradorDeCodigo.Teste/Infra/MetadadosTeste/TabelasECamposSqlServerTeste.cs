using System;
using System.Linq;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Metadados;
using MbUnit.Framework;


namespace GeradorDeCodigo.Teste.Infra.MetadadosTeste
{
    [TestFixture]
    public class TabelasECamposSqlServerTeste
    {

        ITabelasECampos tabelasECamposSqlServer = new TabelasECamposSqlServer();

        public TabelasECamposSqlServerTeste()
        {
            StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS", "Northwind");
            
        }
        

        [Test]
        public void Passando_o_nome_de_uma_tabela_ele_retorna_verdadeiro()
        {


            Assert.IsTrue(tabelasECamposSqlServer.VerificaSeExisteTabela("Categories"));

        }

        [Test]
        public void Passando_o_nome_de_uma_tabela_que_nao_existe_ele_retorna_falso()
        {


            Assert.IsFalse(tabelasECamposSqlServer.VerificaSeExisteTabela("Categorie"));

        }

        [Test]
        public void Retorna_uma_exception_passando_uma_string_vazia()
        {


            Assert.Throws<Exception>(() => tabelasECamposSqlServer.VerificaSeExisteTabela(""));

        }

        [Test]
        public void Retorna_a_quantidade_de_tabelas_do_banco_Northwind()
        {
            Assert.AreEqual(13, tabelasECamposSqlServer.TabelasDoBanco().Count());
        }

        [Test]
        public void Retorna_a_quantidade_de_campos_de_uma_tabelas_do_banco_Northwind()
        {
            const string tabela = "Categories";
            Assert.AreEqual(4, tabelasECamposSqlServer.CamposDaTabela(tabela).Count());
        }

        [Test]
        public void Retorna_a_quantidade_de_campos_chaves_de_uma_tabelas_do_banco_Northwind()
        {
            const string tabela = "Categories";
            Assert.AreEqual(1, tabelasECamposSqlServer.CamposChavesDaTabela(tabela).Count());
        }

        [Test]
        public void Retorna_os_campos_chaves_de_uma_tabelas_do_banco_Northwind()
        {
            const string tabela = "CustomerCustomerDemo";
            string CustomerID = "CustomerID".ToUpper();
            string CustomerTypeID = "CustomerTypeID".ToUpper();
            

            Assert.IsNotEmpty(tabelasECamposSqlServer.CamposDaTabela(tabela).Where(t => t == CustomerID).First());
            Assert.IsNotEmpty(tabelasECamposSqlServer.CamposDaTabela(tabela).Where(t => t == CustomerTypeID).First());
        }


        [Test]
        public void Retorna_verdadeiro_se_campo_for_chave_de_uma_tabelas_do_banco_Northwind()
        {
            const string tabela = "CustomerCustomerDemo";
            string CustomerID = "CustomerID".ToUpper();
            string CustomerTypeID = "CustomerTypeID".ToUpper();

            Assert.IsTrue(tabelasECamposSqlServer.CampoChavesDaTabela(tabela, CustomerTypeID));
            Assert.IsTrue(tabelasECamposSqlServer.CampoChavesDaTabela(tabela, CustomerID));
        }

        [Test]
        public void Retorna_falso_se_campo_for_chave_de_uma_tabelas_do_banco_Northwind()
        {
            const string tabela = "Categories";
            string CustomerID = "CategoryName".ToUpper();
            string CustomerTypeID = "Description".ToUpper();

            Assert.IsFalse(tabelasECamposSqlServer.CampoChavesDaTabela(tabela, CustomerTypeID));
            Assert.IsFalse(tabelasECamposSqlServer.CampoChavesDaTabela(tabela, CustomerID));
        }
        
        [Test]
        public void Retorna_o_dialeto_do_Banco_de_dados()
        {
            Assert.AreEqual("NHibernate.Dialect.MsSql2008Dialect",
                tabelasECamposSqlServer.DialectDoBancodeDados());
        }

        [Test]
        public void Retorna_o_drive_a_ser_usado_pelo_NHibernate()
        {
            Assert.AreEqual("NHibernate.Driver.SqlClientDriver",
                tabelasECamposSqlServer.ConnectionDriver());
        }

        [Test]
        public void Retorna_o_tanho_de_um_campo_string_tabelas_do_banco_Northwind()
        {
            const string tabela = "Customers";
            string CustomerID = "ContactName".ToUpper();

            Assert.AreEqual("30", tabelasECamposSqlServer.TamanhodoCamposString(tabela, CustomerID));

        }

        [Test]
        public void Retorna_o_campo_chave_de_tabela_do_banco_Northwind()
        {
            const string tabela = "Customers";
            string CustomerID = "CustomerID".ToUpper();

            Assert.AreEqual("CustomerID".ToUpper(), tabelasECamposSqlServer.CampoChavesDaTabela(tabela));

        }

        [Test]
        public void Retorna_se_um_campo_e_obrigatorio_de_tabela_do_banco_Northwind()
        {
            const string tabela = "Customers";
            string CustomerID = "CustomerID".ToUpper();  
            string ContactName = "ContactName".ToUpper();

            Assert.IsTrue(tabelasECamposSqlServer.CampoOBrigatorio(tabela, CustomerID));
            Assert.IsFalse(tabelasECamposSqlServer.CampoOBrigatorio(tabela, ContactName));

        }

        [Test]
        public void Passado_uma_tabela_do_banco_Northwind_ele_retorna_seus_relacionamentos()
        {
            const string tabela = "Customers";
            const string relacionamento1 = "Orders";
            const string relacionamento2 = "CustomerCustomerDemo";

            Assert.AreEqual(2, tabelasECamposSqlServer.TabelasOndeARelacionamento(tabela).Count());
            Assert.AreEqual(relacionamento1,
                tabelasECamposSqlServer.TabelasOndeARelacionamento(tabela).Where(t => t == relacionamento1).FirstOrDefault());
            Assert.AreEqual(relacionamento2,
                tabelasECamposSqlServer.TabelasOndeARelacionamento(tabela).Where(t => t == relacionamento2).FirstOrDefault());

        }

        [Test]
        public void Passado_um_campo_e_uma_tabela_do_banco_Northwind_ele_retorna_se_e_Identity()
        {
            const string tabela = "Categories";
            const string campos1 = "CategoryID";
            const string campos2 = "CategoryName";

            Assert.IsTrue(tabelasECamposSqlServer.CamposIdentityDaTabela(tabela, campos1));
            Assert.IsFalse(tabelasECamposSqlServer.CamposIdentityDaTabela(tabela, campos2));
        }

        [Test]
        public void Retorna_o_campo_chaveEstrageira_de_tabela_do_banco_Northwind()
        {
            const string tabela = "CustomerCustomerDemo";
            string campos1 = "CustomerID".ToUpper();
            string campos2 = "CustomerTypeID".ToUpper();



            Assert.AreEqual(2, tabelasECamposSqlServer.CamposChavesEstrageiraDaTabela(tabela).Count());
           
            Assert.AreEqual(campos1,
                tabelasECamposSqlServer.CamposChavesEstrageiraDaTabela(tabela).Where(t => t == campos1).FirstOrDefault().ToUpper());
           
            Assert.AreEqual(campos2, 
                tabelasECamposSqlServer.CamposChavesEstrageiraDaTabela(tabela).Where(t=>t==campos2).FirstOrDefault().ToUpper());
        }

        [Test]
        public void Retorna_a_tabela_do_campo_chaveEstrageira_de_tabela_do_banco_Northwind()
        {
            const string tabela = "EmployeeTerritories";
            string campos1 = "TerritoryID".ToUpper();
            string tabelaRetorno = "Territories".ToUpper();


            Assert.AreEqual(tabelaRetorno,
                tabelasECamposSqlServer.NomeDaTabelaDaChaveEstrgeira(tabela, campos1));

        }

        [Test]
        public void Retorna_o_tipo_dado_do_campo_de_acordo_com_banco_Northwind()
        {
            
            
            const string tabela = "EmployeeTerritories";
            string campos1 = "TerritoryID".ToUpper();
            string tabelaRetorno = "nvarchar".ToUpper();


            Assert.AreEqual(tabelaRetorno,
                tabelasECamposSqlServer.TipoDadoBd(tabela, campos1));

        }
    }

}
