using System;
using MbUnit.Framework;


namespace GeradorDeCodigo.Teste.Domino.TrataNomeTeste
{
   [TestFixture]
    public class TrataNomeTeste
    {
       [Test]
        public void Passado_uma_string_para_minisculo()
        {
            const string nome = "JUlierme";

            var trataNome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual(nome.ToLower(), trataNome.ConverteParaMinisculo(nome));
        }

       [Test]
        public void Passado_uma_string_para_minisculo_e_retira_uderline()
        {
            const string nome = "JUlierme_carvalho";

            var trataNome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual("juliermecarvalho", trataNome.ConverteParaMinisculo(nome));
        }

       [Test]
        public void Passado_uma_string_para_minisculo_e_retira_uderline_e_S_no_fim()
        {
            const string nome = "JUlierme_carvalhos";

            var trataNome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual("juliermecarvalho", trataNome.ConverteParaMinisculo(nome, true));
        }

       [Test]
        public void Passado_uma_string_para_minisculo_e_retira_uderline_e_nao_retira_S_no_fim()
        {
            const string nome = "JUlierme_carvalhos";

            var trataNome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual("juliermecarvalhos", trataNome.ConverteParaMinisculo(nome, false));
        }


        [Test]
        public void Retorna_uma_exception_se_passado_nome_vazio()
        {

            var trataNome = new Dominio.TrataNome.TrataNome();
            
            Assert.Throws<Exception>(() => trataNome.ConverteParaMinisculo(" "));

        }

        [Test]
        public void Retorna_uma_exception_se_passado_nome_vazio_e_retira_s()
        {

            var trataNome = new Dominio.TrataNome.TrataNome();
            
            Assert.Throws<Exception>(() => trataNome.ConverteParaMinisculo(" ", true));

        }

       [Test]
        public void Passado_uma_string_Passa_so_a_primeira_letra_para_maisculo_e_retiro_o_uderline()
        {
            const string nome = "JUlierme_carvalho";

            var tratanome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual("JuliermeCarvalho", tratanome.ConverteParaMaisculo(nome));
        }
       [Test]
        public void Passado_uma_string_Passa_so_a_primeira_letra_para_maisculo_e_retiro_o_uderline_e_o_S()
        {
            const string nome = "JUlierme_carvalhos";

            var tratanome = new Dominio.TrataNome.TrataNome();
            MbUnit.Framework.Assert.AreEqual("JuliermeCarvalho", tratanome.ConverteParaMaisculo(nome, true));
        }


        [Test]
        public void Retorna_uma_exception_se_passado_nome_vazio_converte_para_maiusculo()
        {

            var trataNome = new Dominio.TrataNome.TrataNome();
            Assert.Throws<Exception>(() =>  trataNome.ConverteParaMaisculo(" "));

        }

     
    }
}
