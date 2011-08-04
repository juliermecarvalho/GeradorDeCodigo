using System;
using System.Diagnostics;
using GeradorDeCodigo.Infra.Enun;


namespace GeradorDeCodigo.Dominio.GeraArquivo
{
    public class MoverArquivos
    {

        public void CopiaAsDLLNHibernate(string caminho)
        {
            const string dllNHibernate = @"\DLL NHibernate";

            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] files = System.IO.Directory.GetFiles(appStartPath+dllNHibernate);

            GeraEntidades geraEntidades = new GeraEntidades(TipoDeBancoDados.Sqlserver);
            geraEntidades.CriaDiretorio(caminho+dllNHibernate);

            foreach (string file in files)
            {

                string fileName = System.IO.Path.GetFileName(file);
                string destFile = System.IO.Path.Combine(caminho + dllNHibernate, fileName);
                System.IO.File.Copy(file, destFile, true);
            }            
        }
    }
}
