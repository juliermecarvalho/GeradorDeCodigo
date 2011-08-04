using System.Text;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class GeraRepositorio
    {

        public bool RetiraS { get; set; }
        public bool GeraInterace { get; set; }
        private readonly TrataNome.TrataNome _trataNome = new TrataNome.TrataNome();

        public StringBuilder GeraClasseRepositorioBase(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("using System.Collections.Generic;\n");
            stringBuilder.Append("using System.Linq;\n");

            stringBuilder.Append("using ");
            stringBuilder.Append(nameSpace.Substring(0, nameSpace.Length - 12));
            stringBuilder.Append("NHibernateHelp;\n");

            stringBuilder.Append("using NHibernate;\n");
            stringBuilder.Append("using NHibernate.Linq;\n\n");


            stringBuilder.Append("namespace ");
            stringBuilder.Append(nameSpace);
            
            stringBuilder.Append("\n{\n");

            stringBuilder.Append("\tpublic class RepositorioBase\n");
            stringBuilder.Append("\t{\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual void Save<T>(T obj)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tusing (ISession session = NHibernateHelper.OpenSession())\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\tusing (ITransaction transaction = session.BeginTransaction())\n");
            stringBuilder.Append("\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\ttry\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Save(obj);\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Commit();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Flush();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Close();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t\tcatch (NHibernate.HibernateException)\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Rollback();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t}\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual void Save<T>(T obj, ISession session)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tsession.Save(obj);\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual void Update<T>(T obj)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tusing (ISession session = NHibernateHelper.OpenSession())\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\tusing (ITransaction transaction = session.BeginTransaction())\n");
            stringBuilder.Append("\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\ttry\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Update(obj);\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Commit();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Flush();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Close();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t\tcatch (NHibernate.HibernateException)\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Rollback();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t}\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual void Update<T>(T obj, ISession session) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tsession.Update(obj); \n");
            stringBuilder.Append("\t\t}\n");
            stringBuilder.Append(" \n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual void Delete<T>(T obj)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tusing (ISession session = NHibernateHelper.OpenSession())\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\tusing (ITransaction transaction = session.BeginTransaction())\n");
            stringBuilder.Append("\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\ttry\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Delete(obj);\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Commit();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Flush();\n");
            stringBuilder.Append("\t\t\t\t\t\tsession.Close();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t\tcatch (NHibernate.HibernateException)\n");
            stringBuilder.Append("\t\t\t\t\t{\n");
            stringBuilder.Append("\t\t\t\t\t\ttransaction.Rollback();\n");
            stringBuilder.Append("\t\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t\t}\n");
            stringBuilder.Append("\t\t\t}\n");
            stringBuilder.Append("\t\t}\n");
            
            stringBuilder.Append(" \n");
            
            stringBuilder.Append("\t\tpublic virtual void Delete<T>(T obj, ISession session) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tsession.Delete(obj); \n");
            stringBuilder.Append("\t\t}\n");
            
            
            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic T Get<T>(object id) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tusing (ISession session = NHibernateHelper.OpenSession())\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\tvar toReturn = (T)session.Get(typeof(T), id);\n");
            stringBuilder.Append("\t\t\t\tsession.Close();\n");
            stringBuilder.Append("\t\t\t\treturn toReturn;\n");
            stringBuilder.Append("\t\t\t}\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic T Get<T>(object id, ISession session) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn (T) session.Get(typeof(T), id); \n");
            stringBuilder.Append("\t\t}\n");
            

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual IList<T> Get<T>() \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\tusing (ISession session = NHibernateHelper.OpenSession())\n");
            stringBuilder.Append("\t\t\t{\n");
            stringBuilder.Append("\t\t\t\treturn (from obj in session.Linq<T>()\n");
            stringBuilder.Append("\t\t\t\t        select obj).ToList();\n");
            stringBuilder.Append("\t\t\t}\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual IList<T> Get<T>(ISession session) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn (from obj in session.Linq<T>()\n");
            stringBuilder.Append("\t\t\t        select obj).ToList();\n");
            stringBuilder.Append("\t\t}\n");


            stringBuilder.Append("\t}\n");


            stringBuilder.Append("}\n");

         return stringBuilder;
        }

        public StringBuilder GeraClasseRepositorio(string tabela, string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("using System.Collections.Generic;\n");
            

            stringBuilder.Append("using ");
            stringBuilder.Append(nameSpace.Substring(0, nameSpace.Length - 12));
            if(GeraInterace)
            {
                stringBuilder.Append("Interfaces;\n");

            }
            else
            {
                stringBuilder.Append("Entidades;\n");                
            }

            stringBuilder.Append("using NHibernate;\n\n");
            

          

            stringBuilder.Append("namespace ");
            stringBuilder.Append(nameSpace);

            stringBuilder.Append("\n{\n");

            stringBuilder.Append("\tpublic class Repositorio");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(" : RepositorioBase\n");
            stringBuilder.Append("\t{\n");

            Geracrd(stringBuilder, tabela,"Save");
            Geracrd(stringBuilder, tabela,"Update");
            Geracrd(stringBuilder, tabela,"Delete");

            GeraMedoGet(stringBuilder, tabela);


            stringBuilder.Append("\t}\n");
            stringBuilder.Append("}\n");


            return stringBuilder;


        }

        private void GeraMedoGet(StringBuilder stringBuilder, string tabela)
        {

            string i = string.Empty;

            if (GeraInterace)
            {
                i = "I";
            }
            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual ");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(" Get");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("(object id) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn Get<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(">(id);\n"); 
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual ");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(" Get");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("(object id, ISession session) \n");
            stringBuilder.Append("\t\t{\n");

            stringBuilder.Append("\t\t\treturn Get<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(">(id, session);\n");            

            stringBuilder.Append("\t\t}\n");


            stringBuilder.Append(" \n");


            stringBuilder.Append("\t\tpublic virtual IList<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("> Get");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("() \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn Get<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(">();\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");

            stringBuilder.Append("\t\tpublic virtual IList<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("> Get");
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("(ISession session) \n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t\treturn Get<");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(">(session);\n");
            stringBuilder.Append("\t\t}\n");

            stringBuilder.Append(" \n");
      

        }

        private void Geracrd(StringBuilder stringBuilder,  string tabela, string metodo)
        {
            
            string i = string.Empty;

            if(GeraInterace)
            {
                i = "I";
            }

            stringBuilder.Append(" \n");
            stringBuilder.Append("\t\tpublic virtual void ");
            stringBuilder.Append(metodo);
            stringBuilder.Append("(");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(" ");            
            stringBuilder.Append(_trataNome.ConverteParaMinisculo(tabela, RetiraS));
            stringBuilder.Append(")\n\t\t{\n");
            stringBuilder.Append("\t\t\tbase.");
            stringBuilder.Append(metodo);
            //stringBuilder.Append("<");
            //stringBuilder.Append(i);
            //stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("(");
            stringBuilder.Append(_trataNome.ConverteParaMinisculo(tabela, RetiraS));
            stringBuilder.Append(");\n");
            stringBuilder.Append("\t\t}\n");


            stringBuilder.Append(" \n");
            stringBuilder.Append("\t\tpublic virtual void ");
            stringBuilder.Append(metodo);
            stringBuilder.Append("(");
            stringBuilder.Append(i);
            stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append(" ");
            stringBuilder.Append(_trataNome.ConverteParaMinisculo(tabela, RetiraS));
            stringBuilder.Append(", ISession session)\n\t\t{\n");
            stringBuilder.Append("\t\t\tbase.");
            stringBuilder.Append(metodo);
            //stringBuilder.Append("<");
            //stringBuilder.Append(i);
            //stringBuilder.Append(_trataNome.ConverteParaMaisculo(tabela, RetiraS));
            stringBuilder.Append("(");
            stringBuilder.Append(_trataNome.ConverteParaMinisculo(tabela, RetiraS));
            stringBuilder.Append(", session);\n");
            stringBuilder.Append("\t\t}\n");
       
        
        
        }
    }
}
