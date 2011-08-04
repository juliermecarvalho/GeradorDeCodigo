using System.Text;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class GeraNHibernateHelpe
    {

        public StringBuilder GeraConteudoNHibernateHelpe(string nomeNamespace)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append("using NHibernate;\n");
            stringBuilder.Append("using NHibernate.Cfg;\n\n");
            

            stringBuilder.Append("namespace " + nomeNamespace);
            stringBuilder.Append("\n{\n");
            stringBuilder.Append("\tpublic class NHibernateHelper\n");
            stringBuilder.Append("\t{\n");
            stringBuilder.Append("\t\tprivate static ISessionFactory _sessionFactory;\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\t\tprivate NHibernateHelper()\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\t\tpublic static ISession OpenSession()\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn SessionFactory().OpenSession();\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append("\t\tprivate static ISessionFactory SessionFactory()\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tif (_sessionFactory != null)\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\treturn _sessionFactory;\n");
            stringBuilder.Append("\t\t\t}\n");

            stringBuilder.Append("\t\t\tvar config = new Configuration().Configure();\n");
            stringBuilder.Append("\t\t\t_sessionFactory = config.BuildSessionFactory();\n");
            stringBuilder.Append("\n");
            stringBuilder.Append("\t\t\treturn _sessionFactory;\n");
            stringBuilder.Append("\t\t}\n");
            stringBuilder.Append("\t}\n");
            stringBuilder.Append("}\n");



            return stringBuilder;
        }
    }
}
