using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeradorDeCodigo.Dominio.TrataNome;
using GeradorDeCodigo.Infra.Enun;
using GeradorDeCodigo.Infra.Metadados;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    
    public class GeraEntidades : GeraArquivo
    {

        public bool ESerializable { get; set; }
        public bool GeraClasse { get; set; }
        public bool PropertyAltomaticas { get; set; }
        public bool GeraInterface { get; set; }        
        public bool GeraRepositorio { get; set; }

        
        private IEnumerable<string> _retornaCamposAsscicao = new List<string>();
        private IEnumerable<string> _retornaCamposAsscicaoRepetidos = new List<string>();
        private IEnumerable<string> _camposChavesDaTabela = new List<string>();
        private readonly ITabelasECampos _tabelasECampos;


        public GeraEntidades(TipoDeBancoDados tipoDeBanco)
        {
            _tabelasECampos = TabelasECamposFactory.GetTabelasECampos(tipoDeBanco);

        }

        public StringBuilder ConteudoDaEntitade(IEnumerable<string> campos, string namesPace, string nomeTabela)
        {
            var stringBuilder = new StringBuilder();
            ConvertParaTipoCShap.Interface = GeraInterface;


            _retornaCamposAsscicao = RetornaCamposdaAssociacaoNaoRepeditos(nomeTabela);
            _retornaCamposAsscicaoRepetidos = RetornaCamposdaAssociacaoRepeditos(nomeTabela);
            _camposChavesDaTabela = RetornaCamposChavesDaTabela(nomeTabela);

            GeraUsing(stringBuilder, campos, nomeTabela, namesPace);
            stringBuilder.Append("namespace ");
            stringBuilder.Append(namesPace);
            stringBuilder.Append("\n");
            stringBuilder.Append("{\n");
            if (ESerializable)
            {
                stringBuilder.Append("\t[Serializable]\n");
            }
            stringBuilder.Append("\tpublic class ");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));

            if (GeraInterface)
            {
                stringBuilder.Append(" : I");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            }
            stringBuilder.Append("\n\t{\n");

            if(_camposChavesDaTabela.Count() > 1)
            {
                GeraClasseInterna(stringBuilder, _camposChavesDaTabela, nomeTabela);
            }

            if (!PropertyAltomaticas)
            {
                GeraFields(stringBuilder, campos, nomeTabela);
            }

           // GeraContrutor(stringBuilder, campos, nomeTabela);

            if (PropertyAltomaticas)
            {
                GeraPropetyAutomaticas(stringBuilder, campos, nomeTabela);
            }
            else
            {
                GeraPropety(stringBuilder, campos, nomeTabela);
            }

            
            GeraListaDeAssociacao(stringBuilder, nomeTabela);
            stringBuilder.Append("\t}");
            stringBuilder.Append("\n}");

            return stringBuilder;
        }

   



        private IEnumerable<string> RetornaCamposChavesDaTabela(string nomeTabela)
        {

            return _tabelasECampos.CamposChavesDaTabela(nomeTabela);
        }

        private IEnumerable<string> RetornaCamposdaAssociacaoNaoRepeditos(string nomeTabela)
        {
            

            var tabelas = (List<string>) _tabelasECampos.TabelasOndeARelacionamento(nomeTabela);
           
            IList<string> naorepetido = new List<string>();

            tabelas.ForEach(T =>
                                {
                                    if (tabelas.Where(t => t == T).Count() == 1)
                                    {
                                        naorepetido.Add(T);
                                    }
                                });


            return naorepetido;
        }


        private IEnumerable<string> RetornaCamposdaAssociacaoRepeditos(string nomeTabela)
        {
            

            var tabelas = (List<string>)_tabelasECampos.TabelasOndeARelacionamentoCamposRepetidos(nomeTabela);

            return tabelas;
        }

        private void GeraListaDeAssociacao(StringBuilder stringBuilder, string nomeTabela)
        {

            foreach (var campo in _retornaCamposAsscicao)
            {
                ProrpetyAutomaticasAssociacao(stringBuilder, campo, campo);
            }

            foreach (var campo in _retornaCamposAsscicaoRepetidos)
            {
                string tipodedado = RetornaDuasString(campo)[0];
                string nomedocampo = RetornaDuasString(campo)[1];

                ProrpetyAutomaticasAssociacao(stringBuilder, tipodedado, nomedocampo);
            }
            
        }

  

        private void ProrpetyAutomaticasAssociacao(StringBuilder stringBuilder, string tipodedado, string nomedocampo)
        {
            stringBuilder.Append("\t\tpublic virtual IList<");
            if (GeraInterface)
            {
                stringBuilder.Append("I");
            }

            stringBuilder.Append(TrataNome.ConverteParaMaisculo(tipodedado, RetiraS));
            stringBuilder.Append("> ");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomedocampo, RetiraS));
            stringBuilder.Append("List { get; set; }");
            stringBuilder.Append("\n");
        }


        private IList<string> RetornaDuasString(string campo)
        {
            var t = string.Empty;
            var c = string.Empty;
            bool entrou = true;

            foreach (char c1 in campo)
            {

                if (c1 != ' ' && entrou)
                {
                    t += c1;
                }
                else
                {
                    entrou = false;
                    c += c1;
                }
            }
            var toReturn = new List<string> {t.Trim(), c.Trim()};
            return toReturn;
        }

        /// <summary>
        /// Gera toda a parte e using mais so será gerado os
        /// using nessários para o funcionamento da classe.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        /// <param name="namesPace"></param>
        private void GeraUsing(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela, string namesPace)
        {
            stringBuilder.Append("using System;\n");
            stringBuilder.Append("using System.Collections.Generic;\n");
            
            if(GeraInterface && (!string.IsNullOrWhiteSpace(namesPace)))
            {
                stringBuilder.Append("using " + namesPace.Substring(0, (namesPace.Length - 10)) + ".Interfaces;\n");    
            }
            if (_camposChavesDaTabela.Count() > 1 && (!string.IsNullOrWhiteSpace(namesPace)))
            {
                stringBuilder.Append("using " + namesPace.Substring(0, (namesPace.Length - 10)) + ".Entidades;\n");    
                
            }

            stringBuilder.Append("\n");
        }

        /// <summary>
        /// Gera o conteúdo da Inteface.
        /// </summary>
        /// <param name="campos"></param>
        /// <param name="namesPace"></param>
        /// <param name="nomeTabela"></param>
        /// <returns></returns>
        public StringBuilder ConteudoDaInteface(IEnumerable<string> campos, string namesPace, string nomeTabela)
        {
            var stringBuilder = new StringBuilder();
            ConvertParaTipoCShap.Interface = GeraInterface;
            _retornaCamposAsscicao = RetornaCamposdaAssociacaoNaoRepeditos(nomeTabela);
            _retornaCamposAsscicaoRepetidos = RetornaCamposdaAssociacaoRepeditos(nomeTabela);
            _camposChavesDaTabela = RetornaCamposChavesDaTabela(nomeTabela);

            ESerializable = false;
            
            GeraUsing(stringBuilder, campos, nomeTabela, "");
            stringBuilder.Append("namespace ");
            stringBuilder.Append(namesPace);
            stringBuilder.Append("\n");
            stringBuilder.Append("{\n");

            GeraPropetyPkInterface(stringBuilder, campos, nomeTabela);
           
            stringBuilder.Append("\tpublic interface I");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));

            stringBuilder.Append("\n\t{\n");

            GeraPropetyInterface(stringBuilder, campos, nomeTabela);

            GeraListaDeAssociacaoInterface(stringBuilder, nomeTabela);

            stringBuilder.Append("\t}");
            stringBuilder.Append("\n}");

            return stringBuilder;

        }

        private void GeraPropetyPkInterface(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {

            var entrou = false;                
                foreach (string campo in campos)
                {
                    if (VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                    {
                        if (!entrou)
                        {
                            stringBuilder.Append("\tpublic interface IPk");
                            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
                            stringBuilder.Append("\n\t{\n");
                            entrou = true;
                        }
                        stringBuilder.Append("\t\t");
                        stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                        stringBuilder.Append(" ");
                        stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                        stringBuilder.Append(" { get; set; }");
                        stringBuilder.Append("\n");
                    }

                }
                if (entrou)
                {
                    stringBuilder.Append("\t}\n");
                }

        }

        private void GeraListaDeAssociacaoInterface(StringBuilder stringBuilder, string nomeTabela)
        {
            foreach (var campo in _retornaCamposAsscicao)
            {

                stringBuilder.Append("\t\tIList<");
                stringBuilder.Append("I");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo, RetiraS));
                stringBuilder.Append("> ");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo, RetiraS));
                stringBuilder.Append("List { get; set; }");
                stringBuilder.Append("\n");
            }

            foreach (var campo in _retornaCamposAsscicaoRepetidos)
            {
                string tipodedado = RetornaDuasString(campo)[0];
                string nomedocampo = RetornaDuasString(campo)[1];
                stringBuilder.Append("\t\tIList<");
                if (GeraInterface)
                {
                    stringBuilder.Append("I");
                }

                stringBuilder.Append(TrataNome.ConverteParaMaisculo(tipodedado, RetiraS));
                stringBuilder.Append("> ");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomedocampo, RetiraS));
                stringBuilder.Append("List { get; set; }");
                stringBuilder.Append("\n");
            }
        }

        /// <summary>
        /// Gera as property automáticas.
        /// que é usanda tanto quando gera o conteúdo do classe
        /// quanto da Intefaces
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        private void GeraPropetyAutomaticas(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {

            bool entrou = true;
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringBuilder.Append("\t\tpublic virtual ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" ");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append(" { get; set; }");
                    stringBuilder.Append("\n");
                }
                else
                {
                    if (entrou)
                    {
                        stringBuilder.Append("\t\tpublic virtual ");
                        if (GeraInterface)
                        {
                            stringBuilder.Append("I");

                        }

                        stringBuilder.Append("Pk");
                        stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
                        stringBuilder.Append(" Pk { get; set; }\n");
                        entrou = false;
                    }
                }

            }
            stringBuilder.Append("\n");
        }
        /// <summary>
        /// Gera as property automáticas.
        /// que é usanda tanto quando gera o conteúdo do classe
        /// quanto da Intefaces
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        private void GeraPropetyInterface(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {
            
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringBuilder.Append("\t\t");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" ");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append(" { get; set; }");
                    stringBuilder.Append("\n");
                }

            }
            stringBuilder.Append("\n");
        }
        /// <summary>
        /// Gera os Propety da classe lembrando que esta não
        /// são propety automáticas.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        private void GeraPropety(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {

            bool entrou = true;
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringBuilder.Append("\t\tpublic virtual ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" ");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append("\n\t\t{");
                    stringBuilder.Append("\n\t\t\tget { return _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(";}\n");
                    stringBuilder.Append("\t\t\tset { _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(" = value;}\n");


                    stringBuilder.Append("\t\t}");

                    stringBuilder.Append("\n");
                }
                else
                {
                    if (entrou)
                    {

                        stringBuilder.Append("\t\tpublic virtual ");
                        if (GeraInterface)
                        {
                            stringBuilder.Append("I");

                        }
                        
                        
                        stringBuilder.Append("Pk");
                        stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
                        stringBuilder.Append(" Pk\n");                            
                        
                        stringBuilder.Append("\t\t{\n");
                        stringBuilder.Append("\t\t\tget { return _pk; }\n");
                        stringBuilder.Append("\t\t\tset { _pk = value; }\n");

                        stringBuilder.Append("\t\t}\n");
                        entrou = false;
                    }
                }


            }
            stringBuilder.Append("\n");

        }
        /// <summary>
        /// No caso de uma tabela ter chave composta ele retorna verdadeiro 
        /// se o campo for chave
        /// </summary>
        /// <param name="nomeTabela"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        public bool VerificaSeEChaverQuandoChaveComposta(string nomeTabela, string campo)
        {
           
            var campos = _tabelasECampos.CamposChavesDaTabela(nomeTabela);
            if (campos.Count() > 1)
            {
                if (!string.IsNullOrWhiteSpace(campos.Where(c => c == campo).FirstOrDefault()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gera os fileds da classe
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        /// <returns></returns>
        private void GeraFields(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {

            bool entrou = true;
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringBuilder.Append("\t\tprivate ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(";\n");
                }
                else
                {
                    if (entrou)
                    {
                        stringBuilder.Append("\t\tprivate ");
                       
                        if (GeraInterface)
                        {
                            stringBuilder.Append("I");

                        }


                        stringBuilder.Append("Pk");
                        stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
                        stringBuilder.Append(" _pk;\n");
                        
                        entrou = false;
                    }
                }

            }
            stringBuilder.Append("\n");


        }

        /// <summary>
        /// Gera o construtor da classe.
        /// Um construtor sem parâmetros
        /// e outro parametrizado com todos os campos do Objeto.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="campos"></param>
        /// <param name="nomeTabela"></param>
        /// <returns></returns>
        private void GeraContrutor(StringBuilder stringBuilder, IEnumerable<string> campos, string nomeTabela)
        {


            //Construtor sem parâmetro :
            stringBuilder.Append("\t\tpublic ");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append("()\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n\n");
            //Fim do Construtor sem parâmetro :

            //Contrutor parametrizado
            stringBuilder.Append("\t\tpublic ");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append("(");

            var stringCampos = new StringBuilder();

            var entrou = true;
            //Monta os parâmentros.do construtor.
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringCampos.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringCampos.Append(" ");
                    stringCampos.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringCampos.Append(", ");
                }
                else
                {
                    if (entrou)
                    {
                        if(GeraInterface)
                        {
                            stringBuilder.Append("I");                            
                        }
                        stringCampos.Append("Pk");
                        stringCampos.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
                        stringCampos.Append(" pk, ");
                        entrou = false;
                    }
                }
            }
            if (stringCampos.Length > 0)
            {
                stringBuilder.Append(stringCampos.ToString().Substring(0, stringCampos.ToString().Length - 2));
            }
            stringBuilder.Append(")\n");
            stringBuilder.Append("\t\t{\n");
            entrou = true;
            //Associação de cada parâmentro ao property
            foreach (string campo in campos)
            {
                if (!VerificaSeEChaverQuandoChaveComposta(nomeTabela, campo))
                {
                    stringBuilder.Append("\t\t\t");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append(" = ");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(";\n");
                }
                else
                {
                    if (entrou)
                    {
 
                            stringBuilder.Append("\t\t\tPk = pk;\n");    
                        
                        entrou = false;
                    }
                }
            }

            stringBuilder.Append("\t\t}\n");
            //Fim Contrutor com parametrizado

 

        }
        /// <summary>
        /// Quando uma tabela tem chave composta e gerado uma classe
        /// interna com os campos que representam esta classe.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="camposChavesDaTabela">campos chaves da tabela</param>
        private void GeraClasseInterna(StringBuilder stringBuilder, IEnumerable<string> camposChavesDaTabela, string nomeTabela)
        {
            stringBuilder.Append("\t\tpublic class Pk");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            
            if(GeraInterface)
            {
                stringBuilder.Append(" : IPk");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));

            }
            
           
            stringBuilder.Append("\n\t\t{\n");

            if (!PropertyAltomaticas)
            {
                foreach (string campo in camposChavesDaTabela)
                {
                    stringBuilder.Append("\t\t\tprivate ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(";\n");
                }
                stringBuilder.Append("\n");
            }

            //construtor sem parametros
            stringBuilder.Append("\t\t\tpublic Pk");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append("()\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t}\n");
            //fim construtor sem parametros



            //Contrutor parametrizado            
            stringBuilder.Append("\t\t\tpublic Pk");
            stringBuilder.Append(TrataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append("(");
            var stringCampos = new StringBuilder();


            //Monta os parâmentros.do construtor.
            foreach (string campo in camposChavesDaTabela)
            {
                stringCampos.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                stringCampos.Append(" ");
                stringCampos.Append(TrataNome.ConverteParaMinisculo(campo));
                stringCampos.Append(", ");
            }
            stringBuilder.Append(stringCampos.ToString().Substring(0, stringCampos.ToString().Length - 2));

            stringBuilder.Append(")\n");
            stringBuilder.Append("\t\t\t{\n");

            //Associação de cada parâmentro ao property
            foreach (string campo in camposChavesDaTabela)
            {
                stringBuilder.Append("\t\t\t\t");
                stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                stringBuilder.Append(" = ");
                stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                stringBuilder.Append(";\n");
            }

            stringBuilder.Append("\t\t\t}\n\n");
            //Fim Contrutor com parametrizado



            if (!PropertyAltomaticas)
            {
                foreach (string campo in camposChavesDaTabela)
                {
                    stringBuilder.Append("\t\t\tpublic virtual ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" ");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append("\n\t\t\t{");
                    stringBuilder.Append("\n\t\t\t\tget { return _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(";}\n");
                    stringBuilder.Append("\t\t\t\tset { _");
                    stringBuilder.Append(TrataNome.ConverteParaMinisculo(campo));
                    stringBuilder.Append(" = value;}\n");


                    stringBuilder.Append("\t\t\t}");

                    stringBuilder.Append("\n");

                }

            }
            else
            {
                foreach (string campo in camposChavesDaTabela)
                {
                    stringBuilder.Append("\t\t\tpublic virtual ");
                    stringBuilder.Append(ConvertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(" ");
                    stringBuilder.Append(TrataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append(" { get; set; }");
                    stringBuilder.Append("\n");

                }
                stringBuilder.Append("\n");
                
            }

            GeraEqualsGetHashCode(stringBuilder);

            
            stringBuilder.Append("\t\t}\n\n");

                     
        }

        private void GeraEqualsGetHashCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append("\t\t\tpublic override bool Equals(object obj)\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\treturn base.Equals(obj);\n");
            stringBuilder.Append("\t\t\t}\n\n");

            stringBuilder.Append("\t\t\tpublic override int GetHashCode()\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\treturn base.GetHashCode();\n");
            stringBuilder.Append("\t\t\t}\n");
        }
    }
}
