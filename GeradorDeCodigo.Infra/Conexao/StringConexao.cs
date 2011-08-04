using System.Diagnostics.Contracts;

namespace GeradorDeCodigo.Infra.Conexao
{
    public class StringConexao
    {

        public static string Stringconexao { get; private set; }
        private StringConexao()
        {
            
        }

        public static void GetSqlServer(string source)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(source));

            Stringconexao = @"Data Source=" + source + ";Integrated Security=True";
        }

        public static void GetSqlServer(string source, string catalog)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(source));
            Contract.Requires(!string.IsNullOrWhiteSpace(catalog));
           // Stringconexao =   @"Data Source=VGA-DES\SQLEXPRESS;Initial Catalog=Suppin;Integrated Security=True";
            Stringconexao = @"Data Source=" + source + ";Initial Catalog=" + catalog + ";Integrated Security=True";
        }

        public static void GetOralce(string host, string porta, string servename,string id, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(host));
            Contract.Requires(!string.IsNullOrWhiteSpace(id));
            Contract.Requires(!string.IsNullOrWhiteSpace(porta));
            Contract.Requires(!string.IsNullOrWhiteSpace(servename));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));

            Stringconexao = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + porta + ")))(CONNECT_DATA=(SERVICE_NAME=" + servename + ")));User ID=" + id + ";Password=" + password;
            
        }


        public static void GetSqlServer(string source, string catalog, string id, string password)
        {

            Contract.Requires(!string.IsNullOrWhiteSpace(source));
            Contract.Requires(!string.IsNullOrWhiteSpace(id));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            Contract.Requires(!string.IsNullOrWhiteSpace(catalog));

            Stringconexao = @"Data Source=" + source + ";Initial Catalog=" +catalog + ";User ID=" + id + ";Password=" + password;
        }
    }
}