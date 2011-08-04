using System.Collections.Generic;
using System.Text;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;
using GeradorDeCodigo.Infra.Metadados;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class GeraXmlDeConfigNHibernate
    {
        
        public StringBuilder GeraConteudoXmlDeConfigNHibernate(string nomeNamespace, IEnumerable<string> listaTabelas, bool retiraS)
        {
            var stringBuilder = new StringBuilder();
            var tabelasecampos = TabelasECamposFactory.GetTabelasECampos(TipoDeBancoDados.Sqlserver);
            var tratanome = new TrataNome.TrataNome();

            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            stringBuilder.Append("<hibernate-configuration xmlns=\"urn:nhibernate-configuration-2.2\">\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\n");
            
            stringBuilder.Append("\t<session-factory name= \""+nomeNamespace+"\">\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\t<!-- configuração -->\n");

            stringBuilder.Append("\t\t<property name=\"connection.provider\">NHibernate.Connection.DriverConnectionProvider</property>\n");
            stringBuilder.Append("\t\t<property name=\"dialect\">" + tabelasecampos.DialectDoBancodeDados() + "</property>\n");
            stringBuilder.Append("\t\t<property name=\"connection.driver_class\">" + tabelasecampos.ConnectionDriver() + "</property>\n");
            stringBuilder.Append("\t\t<property name=\"connection.connection_string\">" + StringConexao.Stringconexao + "</property>\n");
            stringBuilder.Append("\t\t<property name=\"proxyfactory.factory_class\">NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu</property>\n");
            stringBuilder.Append("\t\t<property name=\"show_sql\">true</property>\n");
            stringBuilder.Append("\t\t<property name=\"adonet.batch_size\">10</property>\n");
            stringBuilder.Append("\t\t<property name=\"use_outer_join\">true</property>\n");
            stringBuilder.Append("\t\t<property name=\"query.substitutions\">true 1, false 0, yes 'Y', no 'N'</property>\n");
            stringBuilder.Append("\n\n\n");
            stringBuilder.Append("\t<!-- arquivos mapiados -->\n");

            var nomeNamespaceSemEntidade = nomeNamespace.Substring(0, nomeNamespace.Length - 10);
            
            foreach (string tabela in listaTabelas)
            {
                var camposchaver = (List<string>) tabelasecampos.CamposChavesDaTabela(tabela);

                if (camposchaver.Count != 0)
                {
                    stringBuilder.Append("\t\t<mapping resource=\"" + nomeNamespaceSemEntidade + ".Mapeamentos." +
                                         tratanome.ConverteParaMaisculo(tabela, retiraS) + ".hbm.xml\" assembly=\"" +
                                         nomeNamespaceSemEntidade + "\"/>\n");
                }
            }


            stringBuilder.Append("\n");
            stringBuilder.Append("</session-factory>\n");
            stringBuilder.Append("</hibernate-configuration>\n");
            
            
            
            return stringBuilder;
        }
    }
}
