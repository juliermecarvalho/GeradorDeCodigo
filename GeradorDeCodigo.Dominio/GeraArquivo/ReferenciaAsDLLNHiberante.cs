using System.Xml;

namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class ReferenciaAsDLLNHiberante
    {

        public void FazReferenciaAsDllNHibernate(string caminho)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load(caminho);

            XmlNodeList nosItemGroup = doc.GetElementsByTagName("ItemGroup");
            XmlNode no = nosItemGroup[0];

            string valor = @"Antlr3.Runtime, Version=3.1.0.39271, Culture=neutral, PublicKeyToken=3a9cab8f8d22bfb7, processorArchitecture=MSIL";

            XmlNodeList noReference = doc.GetElementsByTagName("Reference");
            foreach (XmlNode i in noReference)
            {
                if (i.Attributes["Include"].Value == valor)
                    return;
            }

            XmlNode novono = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib = doc.CreateAttribute("Include");
            novoAtrib.Value = "Antlr3.Runtime, Version=3.1.0.39271, Culture=neutral, PublicKeyToken=3a9cab8f8d22bfb7, processorArchitecture=MSIL";
            novono.Attributes.Append(novoAtrib);
            XmlNode noFilho = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho.InnerText = "False";
            XmlNode noNeto = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto.InnerText = @"DLL NHibernate\Antlr3.Runtime.dll";
            novono.AppendChild(noFilho);
            novono.AppendChild(noNeto);
            no.AppendChild(novono);


            XmlNode novono1 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib1 = doc.CreateAttribute("Include");
            novoAtrib1.Value = "Castle.Core, Version=1.1.0.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc, processorArchitecture=MSIL";
            novono1.Attributes.Append(novoAtrib1);
            XmlNode noFilho1 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho1.InnerText = "False";
            XmlNode noNeto1 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto1.InnerText = @"DLL NHibernate\Castle.Core.dll";
            novono1.AppendChild(noFilho1);
            novono1.AppendChild(noNeto1);
            no.AppendChild(novono1);


            XmlNode novono2 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib2 = doc.CreateAttribute("Include");
            novoAtrib2.Value = "Castle.DynamicProxy2, Version=2.1.0.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc, processorArchitecture=MSIL";
            novono2.Attributes.Append(novoAtrib2);
            XmlNode noFilho2 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho2.InnerText = "False";
            XmlNode noNeto2 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto2.InnerText = @"DLL NHibernate\Castle.DynamicProxy2.dll";
            novono2.AppendChild(noFilho2);
            novono2.AppendChild(noNeto2);
            no.AppendChild(novono2);


            XmlNode novono3 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib3 = doc.CreateAttribute("Include");
            novoAtrib3.Value = "Iesi.Collections, Version=1.0.1.0, Culture=neutral, PublicKeyToken=aa95f207798dfdb4, processorArchitecture=MSIL";
            novono3.Attributes.Append(novoAtrib3);
            XmlNode noFilho3 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho3.InnerText = "False";
            XmlNode noNeto3 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto3.InnerText = @"DLL NHibernate\Iesi.Collections.dll";
            novono3.AppendChild(noFilho3);
            novono3.AppendChild(noNeto3);
            no.AppendChild(novono3);


            XmlNode novono4 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib4 = doc.CreateAttribute("Include");
            novoAtrib4.Value = "LinFu.DynamicProxy, Version=1.0.3.14911, Culture=neutral, PublicKeyToken=62a6874124340d6e, processorArchitecture=MSIL";
            novono4.Attributes.Append(novoAtrib4);
            XmlNode noFilho4 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho4.InnerText = "False";
            XmlNode noNeto4 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto4.InnerText = @"DLL NHibernate\LinFu.DynamicProxy.dll";
            novono4.AppendChild(noFilho4);
            novono4.AppendChild(noNeto4);
            no.AppendChild(novono4);


            XmlNode novono5 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib5 = doc.CreateAttribute("Include");
            novoAtrib5.Value = "log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821, processorArchitecture=MSIL";
            novono5.Attributes.Append(novoAtrib5);
            XmlNode noFilho5 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho5.InnerText = "False";
            XmlNode noNeto5 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto5.InnerText = @"DLL NHibernate\log4net.dll";
            novono5.AppendChild(noFilho5);
            novono5.AppendChild(noNeto5);
            no.AppendChild(novono5);


            XmlNode novono6 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib6 = doc.CreateAttribute("Include");
            novoAtrib6.Value = "NHibernate, Version=2.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4, processorArchitecture=MSIL";
            novono6.Attributes.Append(novoAtrib6);
            XmlNode noFilho6 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho6.InnerText = "False";
            XmlNode noNeto6 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto6.InnerText = @"DLL NHibernate\NHibernate.dll";
            novono6.AppendChild(noFilho6);
            novono6.AppendChild(noNeto6);
            no.AppendChild(novono6);


            XmlNode novono7 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib7 = doc.CreateAttribute("Include");
            novoAtrib7.Value = "NHibernate.ByteCode.Castle, Version=2.1.2.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4, processorArchitecture=MSIL";
            novono7.Attributes.Append(novoAtrib7);
            XmlNode noFilho7 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho7.InnerText = "False";
            XmlNode noNeto7 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto7.InnerText = @"DLL NHibernate\NHibernate.ByteCode.Castle.dll";
            novono7.AppendChild(noFilho7);
            novono7.AppendChild(noNeto7);
            no.AppendChild(novono7);

            XmlNode novono8 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib8 = doc.CreateAttribute("Include");
            novoAtrib8.Value = "NHibernate.ByteCode.LinFu, Version=2.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4, processorArchitecture=MSIL";
            novono8.Attributes.Append(novoAtrib8);
            XmlNode noFilho8 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho8.InnerText = "False";
            XmlNode noNeto8 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto8.InnerText = @"DLL NHibernate\NHibernate.ByteCode.LinFu.dll";
            novono8.AppendChild(noFilho8);
            novono8.AppendChild(noNeto8);
            no.AppendChild(novono8);


            XmlNode novono9 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib9 = doc.CreateAttribute("Include");
            novoAtrib9.Value = "NHibernate.Linq, Version=1.0.0.4000, Culture=neutral, PublicKeyToken=444cf6a87fdab271, processorArchitecture=MSIL";
            novono9.Attributes.Append(novoAtrib9);
            XmlNode noFilho9 = doc.CreateElement("SpecificVersion", no.NamespaceURI);
            noFilho9.InnerText = "False";
            XmlNode noNeto9 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto9.InnerText = @"DLL NHibernate\NHibernate.Linq.dll";
            novono9.AppendChild(noFilho9);
            novono9.AppendChild(noNeto9);
            no.AppendChild(novono9);

            /*para incluir dll do NHibernate 3.0* /
            XmlNode novono = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib = doc.CreateAttribute("Include");
            novoAtrib.Value = "Antlr3.Runtime";
            novono.Attributes.Append(novoAtrib);

            XmlNode noNeto = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto.InnerText = @"DLL NHibernate\Antlr3.Runtime.dll";
            novono.AppendChild(noNeto);
            no.AppendChild(novono);

            XmlNode novono1 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib1 = doc.CreateAttribute("Include");
            novoAtrib1.Value = "antlr.runtime";
            novono1.Attributes.Append(novoAtrib1);
            XmlNode noNeto1 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto1.InnerText = @"DLL NHibernate\antlr.runtime.dll";
            novono1.AppendChild(noNeto1);
            no.AppendChild(novono1);


            XmlNode novono2 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib2 = doc.CreateAttribute("Include");
            novoAtrib2.Value = "Castle.Core";
            novono2.Attributes.Append(novoAtrib2);
            XmlNode noNeto2 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto2.InnerText = @"DLL NHibernate\Castle.Core.dll";
            novono2.AppendChild(noNeto2);
            no.AppendChild(novono2);


            XmlNode novono3 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib3 = doc.CreateAttribute("Include");
            novoAtrib3.Value = "Common.Logging";
            novono3.Attributes.Append(novoAtrib3);
            XmlNode noNeto3 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto3.InnerText = @"DLL NHibernate\Common.Logging.dll";
            novono3.AppendChild(noNeto3);
            no.AppendChild(novono3);


            XmlNode novono4 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib4 = doc.CreateAttribute("Include");
            novoAtrib4.Value = "Iesi.Collections";
            novono4.Attributes.Append(novoAtrib4);
            XmlNode noNeto4 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto4.InnerText = @"DLL NHibernate\Iesi.Collections.dll";
            novono4.AppendChild(noNeto4);
            no.AppendChild(novono4);


            XmlNode novono5 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib5 = doc.CreateAttribute("Include");
            novoAtrib5.Value = "LinFu.DynamicProxy";
            novono5.Attributes.Append(novoAtrib5);
            XmlNode noNeto5 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto5.InnerText = @"DLL NHibernate\LinFu.DynamicProxy.dll";
            novono5.AppendChild(noNeto5);
            no.AppendChild(novono5);


            XmlNode novono6 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib6 = doc.CreateAttribute("Include");
            novoAtrib6.Value = "NHibernate.ByteCode.Castle";
            novono6.Attributes.Append(novoAtrib6);
            XmlNode noNeto6 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto6.InnerText = @"DLL NHibernate\NHibernate.ByteCode.Castle.dll";
            novono6.AppendChild(noNeto6);
            no.AppendChild(novono6);


            XmlNode novono7 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib7 = doc.CreateAttribute("Include");
            novoAtrib7.Value = "NHibernate.ByteCode.LinFu";
            novono7.Attributes.Append(novoAtrib7);
            XmlNode noNeto7 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto7.InnerText = @"DLL NHibernate\NHibernate.ByteCode.LinFu.dll";
            novono7.AppendChild(noNeto7);
            no.AppendChild(novono7);



            XmlNode novono8 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib8 = doc.CreateAttribute("Include");
            novoAtrib8.Value = "NHibernate.ByteCode.Spring";
            novono8.Attributes.Append(novoAtrib8);
            XmlNode noNeto8 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto8.InnerText = @"DLL NHibernate\NHibernate.ByteCode.Spring.dll";
            novono8.AppendChild(noNeto8);
            no.AppendChild(novono8);


            XmlNode novono9 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib9 = doc.CreateAttribute("Include");
            novoAtrib9.Value = "NHibernate";
            novono9.Attributes.Append(novoAtrib9);
            XmlNode noNeto9 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto9.InnerText = @"DLL NHibernate\NHibernate.dll";
            novono9.AppendChild(noNeto9);
            no.AppendChild(novono9);


            XmlNode novono10 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib10 = doc.CreateAttribute("Include");
            novoAtrib10.Value = "Remotion.Data.Linq";
            novono10.Attributes.Append(novoAtrib10);
            XmlNode noNeto10 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto10.InnerText = @"DLL NHibernate\Remotion.Data.Linq.dll";
            novono10.AppendChild(noNeto10);
            no.AppendChild(novono10);

            XmlNode novono11 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib11 = doc.CreateAttribute("Include");
            novoAtrib11.Value = "Spring.Aop.dll";
            novono11.Attributes.Append(novoAtrib11);
            XmlNode noNeto11 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto11.InnerText = @"DLL NHibernate\Spring.Aop.dll";
            novono11.AppendChild(noNeto11);
            no.AppendChild(novono11);

            XmlNode novono12 = doc.CreateElement("Reference", no.NamespaceURI);
            XmlAttribute novoAtrib12 = doc.CreateAttribute("Include");
            novoAtrib12.Value = "Spring.Core.dll";
            novono12.Attributes.Append(novoAtrib12);
            XmlNode noNeto12 = doc.CreateElement("HintPath", no.NamespaceURI);
            noNeto12.InnerText = @"DLL NHibernate\Spring.Core.dll";
            novono12.AppendChild(noNeto12);
            no.AppendChild(novono12);
            /**/
          
            doc.Save(caminho);

        }
    }
}
