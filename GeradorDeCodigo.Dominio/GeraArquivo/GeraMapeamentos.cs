using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeradorDeCodigo.Dominio.TrataNome;
using GeradorDeCodigo.Infra.Enun;
using GeradorDeCodigo.Infra.Metadados;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class GeraMapeamentos
    {
        public bool Lazy { get; set; }
        public bool RetiraS { get; set; }
        public string NameSpace { get; set; }
        private string _namespaceEntidades;
        private readonly TrataNome.TrataNome _trataNome = new TrataNome.TrataNome();
        private readonly TipoDeBancoDados _tipoDeBancoDados;
        private readonly string _abre = string.Empty;
        private readonly string _fecha = string.Empty;
        private readonly ITabelasECampos _tabelasECampos;
        private IEnumerable<string> _retornaCamposAsscicao = new List<string>();
        private IEnumerable<string> _retornaCamposAsscicaoRepetidos = new List<string>();

        private readonly ConvertParaTipoCShap _convertParaTipoCShap;

        public GeraMapeamentos(TipoDeBancoDados tipoDeBancoDados)
        {
            _tipoDeBancoDados = tipoDeBancoDados;
            _tabelasECampos = TabelasECamposFactory.GetTabelasECampos(_tipoDeBancoDados);
            _convertParaTipoCShap = new ConvertParaTipoCShap(_tipoDeBancoDados);

            if (_tipoDeBancoDados == TipoDeBancoDados.Sqlserver)
            {
                _abre = "[";
                _fecha = "]";
            }
        }


        public StringBuilder GeraMapeamentoDasClasses(string nomeTabela)
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_namespaceEntidades))
            {
                _namespaceEntidades = NameSpace + ".Entidades.";
            }
            _retornaCamposAsscicao = RetornaCamposdaAssociacaoNaoRepeditos(nomeTabela);
            _retornaCamposAsscicaoRepetidos = RetornaCamposdaAssociacaoRepeditos(nomeTabela);

            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");
            stringBuilder.Append("<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\" auto-import=\"true\">\n");
            stringBuilder.Append("\t<class name=\"");
            stringBuilder.Append(_namespaceEntidades);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append(", ");
            stringBuilder.Append(NameSpace);
            stringBuilder.Append("\" table=\"");
            stringBuilder.Append(_abre + nomeTabela.ToUpper() + _fecha);
            stringBuilder.Append("\" lazy=\"");
            stringBuilder.Append(Lazy.ToString().ToLower());
            stringBuilder.Append("\">\n");


            stringBuilder.Append("\n\n\t<!-- chave da entidade -->\n");

            _convertParaTipoCShap.Interface = false;
            GeraId(stringBuilder, nomeTabela);


            stringBuilder.Append("\n\n\t<!-- campos da entidade -->\n");
            GeraCampos(stringBuilder, nomeTabela);

           // stringBuilder.Append("\n\n\t<!-- associação da entidade -->\n");
            GeraAssociação(stringBuilder, nomeTabela);


            stringBuilder.Append("\n");
            stringBuilder.Append("</class>\n");
            stringBuilder.Append("</hibernate-mapping>");


            return stringBuilder;
        }

    
        private IEnumerable<string> RetornaCamposdaAssociacaoNaoRepeditos(string nomeTabela)
        {
            var tabelas = (List<string>)_tabelasECampos.TabelasOndeARelacionamento(nomeTabela);
            
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

        private void GeraAssociação(StringBuilder stringBuilder, string nomeTabela)
        {

           


            foreach (var campo in _retornaCamposAsscicao)
            {

                string tipodedado = RetornaDuasString(campo)[0];


                stringBuilder.Append("\n\t\t<bag name=\"");
                stringBuilder.Append(_trataNome.ConverteParaMaisculo(tipodedado, RetiraS));
                stringBuilder.Append("List\" inverse=\"true\" lazy=\"");
                stringBuilder.Append(Lazy.ToString().ToLower());
                stringBuilder.Append("\" cascade=\"all\">\n");
                stringBuilder.Append("\t\t\t<key column=\"");
                stringBuilder.Append(_tabelasECampos.NomeDoCampoOndeARelacionmento(campo, nomeTabela).ToUpper());
                stringBuilder.Append("\"/>\n");
                stringBuilder.Append("\t\t\t<one-to-many class=\"");
                stringBuilder.Append(_namespaceEntidades);
                stringBuilder.Append(_trataNome.ConverteParaMaisculo(tipodedado, RetiraS));
                stringBuilder.Append("\"/>\n");

                stringBuilder.Append("\t\t</bag>");

            }

            foreach (var campo in _retornaCamposAsscicaoRepetidos)
            {
                string tipodedado = RetornaDuasString(campo)[0];
                string nomedocampo = RetornaDuasString(campo)[1];



                stringBuilder.Append("\n\t\t<bag name=\"");
                stringBuilder.Append(_trataNome.ConverteParaMaisculo(nomedocampo, RetiraS));
                stringBuilder.Append("List\" inverse=\"true\" lazy=\"");
                stringBuilder.Append(Lazy.ToString().ToLower());
                stringBuilder.Append("\" cascade=\"all\">\n");
                stringBuilder.Append("\t\t\t<key column=\"");
                stringBuilder.Append((nomedocampo).ToUpper());
                stringBuilder.Append("\"/>\n");
                stringBuilder.Append("\t\t\t<one-to-many class=\"");
                stringBuilder.Append(_namespaceEntidades);
                stringBuilder.Append(_trataNome.ConverteParaMaisculo(tipodedado, RetiraS));
                stringBuilder.Append("\"/>\n");
                
                stringBuilder.Append("\t\t</bag>");

            }

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
            var toReturn = new List<string> { t.Trim(), c.Trim() };
            return toReturn;
        }

 

        private void GeraCampos(StringBuilder stringBuilder, string nomeTabela)
        {
            var campos = (List<string>)_tabelasECampos.CamposDaTabela(nomeTabela);

            foreach (string campo in campos)
            {
                if (!_tabelasECampos.CampoChavesDaTabela(nomeTabela, campo))
                {
                    var tipoCSharp = _convertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS);

                    //se não for um tipo c# faz referencia a outra class então
                    // many-to-one
                    if (_convertParaTipoCShap.VerificaSeTipoCSharp(tipoCSharp))
                    {
                        stringBuilder.Append("\t\t<property name=\"");
                        //tipo byte[] tem ser convertido para binaryblob.
                        if (tipoCSharp == "byte[]")
                        {
                            tipoCSharp = "BinaryBlob";
                        }
                    }
                    else
                    {
                        stringBuilder.Append("\t\t<many-to-one name=\"");
                    }


                    stringBuilder.Append(_trataNome.ConverteParaMaisculo(campo));
                    stringBuilder.Append("\" column= \"");
                    stringBuilder.Append(campo.ToUpper());
                    

                    //se não é um tipo c# tem que colocar o namespace da entidade.
                    if (!_convertParaTipoCShap.VerificaSeTipoCSharp(tipoCSharp))
                    {


                        stringBuilder.Append("\" class=\"");
                        stringBuilder.Append(_namespaceEntidades);
                        stringBuilder.Append(_convertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                        stringBuilder.Append(", ");
                        stringBuilder.Append(NameSpace);
                        

                    }
                    else
                    {
                        stringBuilder.Append("\" type=\"");
                        stringBuilder.Append(tipoCSharp);

                    }

                    
                    stringBuilder.Append("\" not-null=\"");

                    stringBuilder.Append(_tabelasECampos.CampoOBrigatorio(nomeTabela, campo).ToString().ToLower());
                    stringBuilder.Append("\"/>\n");

                }
            }

        }

        private void GeraId(StringBuilder stringBuilder, string nomeTabela)
        {

            var campos = (List<string>) _tabelasECampos.CamposChavesDaTabela(nomeTabela);

            //Quando tem só um chaver primaria
            if (campos.Count == 0)
            {
                stringBuilder.Append("\t\t<id >\n\t\t</id>");
            }
            else
            {


                if (campos.Count() == 1)
                {
                    QuandoTemUmaChavePrimaria(nomeTabela, campos, stringBuilder);
                }
                else
                {
                    QuandoTemMaisdeUmaChavePrimaria(nomeTabela, campos, stringBuilder);
                }
            }


        }

        private void QuandoTemMaisdeUmaChavePrimaria(string nomeTabela, List<string> campos, StringBuilder stringBuilder)
        {
            stringBuilder.Append("\t\t<composite-id name=\"Pk\" class=\"");
            stringBuilder.Append(_namespaceEntidades);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append("+Pk");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(nomeTabela, RetiraS));
            stringBuilder.Append(", ");
            stringBuilder.Append(NameSpace);
            stringBuilder.Append("\">\n");

            foreach (string campo in campos)
            {
                stringBuilder.Append("\t\t\t");
                var tipoCSharp = _convertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS);

                //se não for um tipo c# faz referencia a outra class então
                // many-to-one
                if (_convertParaTipoCShap.VerificaSeTipoCSharp(tipoCSharp))
                {
                    stringBuilder.Append("\t\t<key-property name=\"");
                    //tipo byte[] tem ser convertido para binaryblob.
                    if (tipoCSharp == "byte[]")
                    {
                        tipoCSharp = "BinaryBlob";
                    }
                }
                else
                {
                    stringBuilder.Append("\t\t<key-many-to-one name=\"");
                }

                stringBuilder.Append(_trataNome.ConverteParaMaisculo(campo));
                stringBuilder.Append("\" column= \"");
                stringBuilder.Append(campo.ToUpper());


                //se não é um tipo c# tem que colocar o namespace da entidade.
                if (!_convertParaTipoCShap.VerificaSeTipoCSharp(tipoCSharp))
                {
                    stringBuilder.Append("\" class=\"");
                    stringBuilder.Append(_namespaceEntidades);
                    stringBuilder.Append(_convertParaTipoCShap.TipoCShap(nomeTabela, campo, RetiraS));
                    stringBuilder.Append(", ");
                    stringBuilder.Append(NameSpace);
                    stringBuilder.Append("\"");

                }
                else
                {
                    stringBuilder.Append("\" type=\"");
                    stringBuilder.Append(tipoCSharp);
                    stringBuilder.Append("\"");

                }
                stringBuilder.Append("/>\n");


            }


            stringBuilder.Append("\t\t</composite-id>");


        }

        private void QuandoTemUmaChavePrimaria(string nomeTabela, List<string> campos, StringBuilder stringBuilder)
        {
            stringBuilder.Append("\t\t<id name=\"");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(campos[0]));
            stringBuilder.Append("\" column=\"");
            stringBuilder.Append(campos[0].ToUpper());
            stringBuilder.Append("\" type=\"");
            stringBuilder.Append(_convertParaTipoCShap.TipoCShap(nomeTabela, campos[0], RetiraS));
            stringBuilder.Append("\">\n");


            if (_tabelasECampos.CamposIdentityDaTabela(nomeTabela, campos[0]))
            {
                stringBuilder.Append("\t\t\t<generator class=\"native\" />\n");
            }
            else
            {
                stringBuilder.Append("\t\t\t<generator class=\"assigned\" />\n");
            }
            stringBuilder.Append("\t\t</id>");
        }
    }
}
