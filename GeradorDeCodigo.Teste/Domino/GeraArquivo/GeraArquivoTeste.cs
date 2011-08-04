using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeradorDeCodigo.Dominio.GeraArquivo;
using GeradorDeCodigo.Dominio.TrataNome;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;
using MbUnit.Framework;


namespace GeradorDeCodigo.Teste.Domino.GeraArquivo
{
    [TestFixture]
    public class GeraArquivoTeste
    {
        private const string _caminho = @"C:\Teste";
        private const string _caminhoInterface = @"\Interface\";
        private const string _caminhoEntity = @"\Entidades\";
       
        [Test]
        public void verifica_se_existe_diretorio()
        {
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            bool verifica = geraEntidades.VerificaSeDiretorioExiste(@"C:\Temp");
            Assert.AreEqual(true, verifica);
        }

        [Test]
        public void verifica_se_existe_diretorio_e_cria()
        {


            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            geraEntidades.CriaDiretorio(_caminho + _caminhoEntity);
            bool verifica = geraEntidades.VerificaSeDiretorioExiste(_caminho+_caminhoEntity);
            Assert.AreEqual(true, verifica);
        }


        [Test]
        public void dado_uma_lista_string_cria_arquivo_no_diretorio()
        {
            Assert.AreEqual(3, RetornaQuantidadeDeArquivosDeUmDiretorio(_caminho+@"\Entidades"));
        }

        private int RetornaQuantidadeDeArquivosDeUmDiretorio(string caminho)
        {
            string[] arquivos = Directory.GetFiles(caminho);
            return arquivos.Count();
        }

        [Test]
        public void passado_uma_lista_string_cria_os_arquivos()
        {
            IList<string> tabelas = new List<string>();
            tabelas.Add("TESTE");
            tabelas.Add("Meu_teste");
            tabelas.Add("teste_meus");
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            geraEntidades.RetiraS = true;
            geraEntidades.CriaEntitades(_caminho+_caminhoEntity, tabelas);
            Assert.AreEqual(3, RetornaQuantidadeDeArquivosDeUmDiretorio(_caminho + _caminhoInterface));
        }
        [Test]
        public void passado_uma_lista_string_cria_as_interface()
        {
            IList<string> tabelas = new List<string>();
            tabelas.Add("I_TESTE");
            tabelas.Add("I_Meu_teste");
            tabelas.Add("I_teste_meus");
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            geraEntidades.CriaEntitades(_caminho + _caminhoInterface, tabelas);
            geraEntidades.GeraInterface = true;
            Assert.AreEqual(3, RetornaQuantidadeDeArquivosDeUmDiretorio(_caminho + _caminhoInterface));
        }

        [Test]
        public void Retorna_uma_exception_para_cria_arquivo_se_caminho_vazinho()
        {
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            Assert.Throws<Exception>(() => geraEntidades.CriaEntitades("", new List<string>()));

        }

        [Test]
        public void Retorna_uma_exception_para_cria_diretorio_se_caminho_vazinho()
        {
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            Assert.Throws<Exception>(() => geraEntidades.CriaDiretorio(""));

        }

        [Test]
        public void passado_um_arquivo_e_uma_StringBild_ele_escreve_no_arquivo()
        {
            const string var1= "julierme";
            const string var2 = "carvalho";

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(var1);
            stringBuilder.Append("\n");
            stringBuilder.Append(var2);
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            geraEntidades.EscreveNoArquivo(_caminho + _caminhoEntity + @"\MeuTeste.cs", stringBuilder);
            Assert.AreEqual(var1 + var2, retorna_o_conteudo_do_arquivo(_caminho + _caminhoEntity + @"\MeuTeste.cs"));
        }

        private string retorna_o_conteudo_do_arquivo(string caminho)
        {
            var streamReader = new StreamReader(caminho);

            string linha = streamReader.ReadLine();
            var toReturn = new StringBuilder();

            while (linha != null)
            {
                
                toReturn.Append(linha);
                linha = streamReader.ReadLine();
            }
             streamReader.Close();
            return toReturn.ToString();
        }

        [Test]
        public void passando_lista_campos_ele_retorna_StringBilderpropetyAutomaticas_true()
        {
            
            const string nomeTabela = "Teste";
            const string namesPace = "julierme.carvalho";
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            IList<string > camopos = new List<string> {"julierme", "CARVALHO"};
            geraEntidades.PropertyAltomaticas = true;
            geraEntidades.GeraInterface = true;
            geraEntidades.ESerializable = true;
            geraEntidades.EscreveNoArquivo(_caminho + _caminhoEntity + @"\" +nomeTabela + ".cs", geraEntidades.ConteudoDaEntitade(camopos, namesPace, nomeTabela));
            geraEntidades.EscreveNoArquivo(_caminho + _caminhoInterface + @"\I" + nomeTabela + ".cs", geraEntidades.ConteudoDaInteface(camopos, namesPace, nomeTabela));

        }
        [Test]
        public void passando_lista_campos_ele_retorna_StringBilder_propetyAutomaticas_false()
        {
            var stringBuilder = new StringBuilder();
            const string nomeTabela = "MeuTeste";
            const string namesPace = "julierme.carvalho";
            var geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            IList<string> camopos = new List<string> { "julierme", "CARVALHO" };
            geraEntidades.PropertyAltomaticas = false;
            geraEntidades.GeraInterface = false;
            geraEntidades.EscreveNoArquivo(_caminho + _caminhoEntity + @"\" + nomeTabela + ".cs", geraEntidades.ConteudoDaEntitade(camopos, namesPace, nomeTabela));

        }
        [Test]
        public void passa_um_tipo_do_banco_de_dados_retorna_um_tipo_do_CShap()
        {
            string TipoBd = "Varchar";
            var convertParaTipoCShap = new ConvertParaTipoCShap(TipoDeBancoDados.Sqlserver);
            Assert.AreEqual("string", convertParaTipoCShap.TipoCShap(TipoBd));
            TipoBd = "int";
            Assert.AreEqual("int", convertParaTipoCShap.TipoCShap(TipoBd));
            TipoBd = "bigint";
            Assert.AreEqual("long", convertParaTipoCShap.TipoCShap(TipoBd));
        }

        [Test]
        public void Retorna_uma_exception_para_tipo_vazinho()
        {
            const string TipoBd = "";
            var convertParaTipoCShap = new ConvertParaTipoCShap(TipoDeBancoDados.Sqlserver);

            Assert.Throws<Exception>(() => convertParaTipoCShap.TipoCShap(TipoBd));

        }

        [Test]
        public void Retorna_uma_exception_para_tipo_que_nao_seja_bd()
        {
            const string TipoBd = "scharp";
            var convertParaTipoCShap = new ConvertParaTipoCShap(TipoDeBancoDados.Sqlserver);

            Assert.Throws<Exception>(() => convertParaTipoCShap.TipoCShap(TipoBd));

        }

       [Test]
       public void Retorna_o_tipo_de_dado_Csharp_classe_reverente()
       {
           StringConexao.GetSqlServer(@"VGA-DES\SQLEXPRESS", "Northwind");
           var convertParaTipoCShap = new ConvertParaTipoCShap(TipoDeBancoDados.Sqlserver);
            
            string Tabela = "EmployeeTerritories";
            string campo1 = "TerritoryID";
            string campo2 = "EmployeeID";

            Assert.AreEqual("Territories".ToUpper(), convertParaTipoCShap.TipoCShap(Tabela, campo1, true));
            Assert.AreEqual("Employees".ToUpper(), convertParaTipoCShap.TipoCShap(Tabela, campo2, true)); 

            Tabela = "Employees";
            campo1 = "LastName";
            campo2 = "Photo";

            Assert.AreEqual("string", convertParaTipoCShap.TipoCShap(Tabela, campo1, true));
            Assert.AreEqual("byte[]", convertParaTipoCShap.TipoCShap(Tabela, campo2, true));
            Assert.AreEqual("Employees".ToUpper(), convertParaTipoCShap.TipoCShap(Tabela, "ReportsTo", true)); 


           Tabela = "Orders";
           campo1 = "OrderID";
           campo2 = "CustomerID";

           Assert.AreEqual("int", convertParaTipoCShap.TipoCShap(Tabela, campo1, true));
           Assert.AreEqual("Customers".ToUpper(), convertParaTipoCShap.TipoCShap(Tabela, campo2, true));
           Assert.AreEqual("Employees".ToUpper(), convertParaTipoCShap.TipoCShap(Tabela, "EmployeeID", true));
           Assert.AreEqual("DateTime", convertParaTipoCShap.TipoCShap(Tabela, "OrderDate", true));
           Assert.AreEqual("decimal", convertParaTipoCShap.TipoCShap(Tabela, "Freight", true)); 
       }
    }

  
}
