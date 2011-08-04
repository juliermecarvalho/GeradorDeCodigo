using System.Diagnostics.Contracts;
using GeradorDeCodigo.Infra.Enun;
using GeradorDeCodigo.Infra.Metadados;
using System.Linq;

namespace GeradorDeCodigo.Dominio.TrataNome
{
    public class ConvertParaTipoCShap
    {

        private TipoDeBancoDados _tipoDeBancoDados;
        
        public bool Interface { get; set; }
        public ConvertParaTipoCShap(TipoDeBancoDados tipoDeBancoDados)
        {
            _tipoDeBancoDados = tipoDeBancoDados;
        }
        public string TipoCShap(string tabela, string campo, bool retiraS)
        {

            ITabelasECampos tabelasECampos = TabelasECamposFactory.GetTabelasECampos(_tipoDeBancoDados);

            var result = tabelasECampos.CamposChavesEstrageiraDaTabela(tabela).Where(t => t == campo.ToUpper()).FirstOrDefault();
            
            if(!string.IsNullOrWhiteSpace(result))
            {
                TrataNome trataNome = new TrataNome();

                if (Interface)
                {
                    return "I" +
                           trataNome.ConverteParaMaisculo(tabelasECampos.NomeDaTabelaDaChaveEstrgeira(tabela, result),
                                                          retiraS);
                }
                
                return trataNome.ConverteParaMaisculo(tabelasECampos.NomeDaTabelaDaChaveEstrgeira(tabela, result),
                                                      retiraS);


            }
            result = tabelasECampos.TipoDadoBd(tabela, campo);
            return TipoCShap(result);
        }
        
        public string TipoCShap(string tipoBd)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(tipoBd));

            tipoBd = tipoBd.ToUpper();
            var toReturn = string.Empty;

            if (tipoBd == "INT" || tipoBd == "SMALLINT" || tipoBd == "tinyint".ToUpper())
            {
                toReturn = "int";
            }
            else
                if (tipoBd == "VARCHAR2" || tipoBd == "NTEXT" || tipoBd == "VARCHAR" || tipoBd == "NVARCHAR2"
                    || tipoBd == "NVARCHAR" || tipoBd == "NCHAR" || tipoBd == "CHAR"
                    || tipoBd == "SYSNAME")
                {
                    toReturn = "string";
                }
                else
                    if (tipoBd == "TIMESTAMP" || tipoBd == "DATE"
                        || tipoBd == "DATETIME" || tipoBd == "SMALLDATETIME")
                    {
                        toReturn = "DateTime";
                    }
                    else
                        if (tipoBd == "BIGINT")
                        {
                            toReturn = "long";
                        }
                        else
                            if (tipoBd == "LONG" || tipoBd == "REAL" || tipoBd == "DECIMAL"
                                || tipoBd == "NUMBER" || tipoBd == "MONEY" || tipoBd == "NUMERIC"
                                || tipoBd == "REAL" || tipoBd == "SMALLMONEY" || tipoBd == "MOEDA" || tipoBd == "SALDOS")
                            {
                                toReturn = "decimal";
                            }
                            else
                                if (tipoBd == "FLOAT")
                                {
                                    toReturn = "double";
                                }
                                else
                                    if (tipoBd == "LONG RAW" || tipoBd == "CLOB" || tipoBd == "IMAGE"
                                        || tipoBd == "NCLOB" || tipoBd == "BLOB" || tipoBd == "BINARY"
                                        || tipoBd == "VARBINARY")
                                    {
                                        toReturn = "byte[]";
                                    }
                                    else
                                        if (tipoBd == "BIT")
                                        {
                                            toReturn = "bool";
                                        }
           
            
            Contract.Assert(!string.IsNullOrWhiteSpace(toReturn));

            return toReturn;
        }

        public bool VerificaSeTipoCSharp(string tipo)
        {
           
            if(tipo == "int" || tipo == "string" || 
                tipo == "DateTime" || tipo == "long" ||
                tipo == "decimal" || tipo == "double" ||
                tipo == "byte[]" || tipo == "bool" || tipo == "BinaryBlob")
            {
                return true;
            }
            return false;
        }
    }
}
