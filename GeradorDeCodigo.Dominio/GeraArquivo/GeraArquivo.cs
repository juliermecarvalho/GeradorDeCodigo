using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml;
using GeradorDeCodigo.Dominio.TrataNome;
using GeradorDeCodigo.Infra.Enun;


namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class GeraArquivo
    {

        public bool RetiraS { get; set; }

        protected readonly ConvertParaTipoCShap ConvertParaTipoCShap = new ConvertParaTipoCShap(TipoDeBancoDados.Sqlserver);
        protected readonly TrataNome.TrataNome TrataNome = new TrataNome.TrataNome();


 
        public bool VerificaSeDiretorioExiste(string caminho)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(caminho));

            var directoryInfo = new DirectoryInfo(caminho);
            return directoryInfo.Exists;
        }

        public void CriaDiretorio(string caminho)
        {
           if(!VerificaSeDiretorioExiste(caminho) )
           {
               Directory.CreateDirectory(caminho);
           }

        }


        private void CriaArquivoNoDiretorio(string caminho, IEnumerable<string> tabelas, bool EInterface)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(caminho));

            this.CriaDiretorio(caminho);
            foreach (string tabela in tabelas)
            {
                string caminhoCompleto;
                if(EInterface)
                {
                    caminhoCompleto = caminho + @"\I" + this.TrataNome.ConverteParaMaisculo(tabela, RetiraS) + ".cs";
                }
                else
                {
                    caminhoCompleto = caminho + @"\" + this.TrataNome.ConverteParaMaisculo(tabela, RetiraS) + ".cs";
                }

                if (!File.Exists(caminhoCompleto))
                {
                    File.Create(caminhoCompleto);
                }
            }
        }

        public void CriaEntitades(string caminho, IEnumerable<string> tabelas)
        {
            this.CriaArquivoNoDiretorio(caminho, tabelas, false);
        }

        public void CriaInterface(string caminho, IEnumerable<string> tabelas)
        {
            this.CriaArquivoNoDiretorio(caminho, tabelas, true);
        }

        public void EscreveNoArquivo(string caminho, StringBuilder stringBuilder)
        {
            var aFile = new StreamWriter(caminho, false, Encoding.ASCII);
            aFile.Write(stringBuilder.ToString());
            aFile.Close();
        }

        public void IncluirArquivoNoProjeto(string caminho, string arquivo, string nomedoNoFilho)
        {
                string filename = caminho;

                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                XmlNodeList nosItemGroup = doc.GetElementsByTagName("ItemGroup");
                //Recupera o nome do assembly

                XmlNodeList nosPropertyGroup = doc.GetElementsByTagName("TargetFrameworkProfile");

                foreach (XmlNode no in nosPropertyGroup)
                {
                    if(no.ChildNodes.Count > 0)
                    {
                        no.FirstChild.InnerText = string.Empty;
                        no.LastChild.InnerText = string.Empty;
                        doc.Save(filename);

                    }
                }

                foreach (XmlNode no in nosItemGroup)
                {

                    /*para não incluir um arquivo duas vezes no projeto!!!*/
                    bool entra = true;

                    XmlNodeList node = doc.GetElementsByTagName(nomedoNoFilho);
                    foreach (XmlNode i in node)
                    {
                        if (i.Attributes["Include"].Value == arquivo)
                            entra = false;
                    }
                    if (entra)
                    {
                        /*para incluir um arquivo no projeto!!!*/
                        XmlNode novono = doc.CreateElement(nomedoNoFilho, no.NamespaceURI);
                        XmlAttribute novoAtrib = doc.CreateAttribute("Include");
                        novoAtrib.Value = arquivo;
                        novono.Attributes.Append(novoAtrib);
                        if (nomedoNoFilho == "Content")
                        {
                            XmlNode noFilho = doc.CreateElement("CopyToOutputDirectory", no.NamespaceURI);
                            noFilho.InnerText = "Always";

                            novono.AppendChild(noFilho);
                        }
                        no.AppendChild(novono);
                        doc.Save(filename);
                    }

                }
          
        }

    }
}
