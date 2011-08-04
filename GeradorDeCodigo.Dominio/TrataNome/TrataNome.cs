using System.Diagnostics.Contracts;
using System.Globalization;

namespace GeradorDeCodigo.Dominio.TrataNome
{
    public class TrataNome
    {
        public string ConverteParaMinisculo(string nome)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(nome));

            nome = nome.Replace("_", string.Empty).ToLower();
            return nome.Replace(" ", string.Empty);
        }

        public string ConverteParaMinisculo(string nome, bool retirar)
        {
            if (retirar)
            {
                nome = RetiraSNoFim(nome);
            }
            return ConverteParaMinisculo(nome);
        }

        private string RetiraSNoFim(string nome)
        {
           
            Contract.Requires(!string.IsNullOrWhiteSpace(nome));


            if (nome.Substring(nome.Length - 1, 1) == "s" ||
                nome.Substring(nome.Length - 1, 1) == "S")
            {
                nome = nome.Remove(nome.Length - 1);
            }

            return nome;

        }

        public string ConverteParaMaisculo(string nome)
        {

            Contract.Requires(!string.IsNullOrWhiteSpace(nome));
            
            CultureInfo cultureinfo = System.Threading.Thread.CurrentThread.CurrentCulture;

            nome = nome.Replace("_", " ").ToLower();
            nome = cultureinfo.TextInfo.ToTitleCase(nome);
            return nome.Replace(" ", string.Empty);

        }

        public string ConverteParaMaisculo(string nome, bool retirar)
        {
            if (retirar)
            {
                nome = RetiraSNoFim(nome);
            }
            return ConverteParaMaisculo(nome);
        }
    }
}