using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GeradorDeCodigo.Dominio.GeraArquivo;
using GeradorDeCodigo.Dominio.TrataNome;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;
using GeradorDeCodigo.Infra.Metadados;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace GeradorDeCodigo.App
{
   

    public partial class MainWindow : Window
    {
        private string _namesPace = string.Empty;
        private string _caminho = string.Empty;
        private string _caminhoDoArquivoProj = string.Empty;
        private bool _lazy;
        private delegate void UpdateUi();
        private readonly IList<string> _listacomastabelas = new List<string>();
        private readonly TrataNome _trataNome = new TrataNome();
        private GeraEntidades _geraEntidades;
        private TipoDeBancoDados _tipoDeBanco;


        public MainWindow()
        {
            InitializeComponent();

        }

        private void chkAuthentication_Click(object sender, RoutedEventArgs e)
        {
            if (chkAuthentication.IsChecked != null)
            {
                txtLoginSqlServer.IsEnabled = !chkAuthentication.IsChecked.Value;
                txtSenhaSqlServer.IsEnabled = !chkAuthentication.IsChecked.Value;
            }
        }

        private void btnGeraArquivos_Click(object sender, RoutedEventArgs e)
        {
            ListaComaAsTabelas();
            _tipoDeBanco = RetornaTipoDeBancoDeDados();
            _geraEntidades = new GeraEntidades(_tipoDeBanco);

            if (_listacomastabelas.Count() > 0)
            {
                var openFileDialog = new OpenFileDialog();

                openFileDialog.DefaultExt = ".csproj";
                openFileDialog.Filter = "csproj files (*.csproj)|*.csproj";

                var result = openFileDialog.ShowDialog();

                if (result == true)
                {

                    _caminhoDoArquivoProj = openFileDialog.FileName;
                    _namesPace = openFileDialog.SafeFileName;
                    _caminho = _caminhoDoArquivoProj.Substring(0, _caminhoDoArquivoProj.Length - _namesPace.Length);
                    _namesPace = _namesPace.Substring(0, _namesPace.Length - 7);

                    SetaPropriedadesDeGeraEntidade();

                    _lazy = chkLazy.SelectedIndex == 0;

                    if (!string.IsNullOrWhiteSpace(_caminho))
                    {
                        var trd = new Thread(GeraOsArquivos) {IsBackground = true};
                        trd.Start();
                    }

                    GeracaoDaClasseNHibernateHelp();

                    
                    var moverArquivos = new MoverArquivos();
                    moverArquivos.CopiaAsDLLNHibernate(_caminho);
                    
                    
                    var referenciaAsDllnHiberante = new ReferenciaAsDLLNHiberante();
                    referenciaAsDllnHiberante.FazReferenciaAsDllNHibernate(_caminhoDoArquivoProj);
                    

                }

            }
            else
            {
                MessageBox.Show("É necessário uma ou mais tabelas.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        private void GeracaoDaClasseNHibernateHelp()
        {
            if (chkNHibernateHelp.IsChecked != null)
            {
                if (chkNHibernateHelp.IsChecked.Value)
                {
                    const string nHibernateHelp = @"NHibernateHelp\";
                    var namesPaceEntidades = _namesPace + ".NHibernateHelp";


                    var geraNHibernateHelpe = new GeraNHibernateHelpe();
                    _geraEntidades.CriaDiretorio(_caminho + nHibernateHelp);
                    _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj,
                                                           nHibernateHelp + "NHibernateHelper.cs",
                                                           "Compile");
                    _geraEntidades.EscreveNoArquivo(_caminho + nHibernateHelp + "NHibernateHelper.cs",
                                                    geraNHibernateHelpe.GeraConteudoNHibernateHelpe(
                                                        namesPaceEntidades));

                }
            }
        }

        private void SetaPropriedadesDeGeraEntidade()
        {
            if (chkGeraClasse.IsChecked != null)
            {
                _geraEntidades.GeraClasse = chkGeraClasse.IsChecked.Value;
            }
            if (chkGeraInterface.IsChecked != null)
            {
                _geraEntidades.GeraInterface = chkGeraInterface.IsChecked.Value;
            }
            if (chkPropertyAutomaticas.IsChecked != null)
            {
                _geraEntidades.PropertyAltomaticas = chkPropertyAutomaticas.IsChecked.Value;
            }
            if (chkSerializable.IsChecked != null)
            {
                _geraEntidades.ESerializable = chkSerializable.IsChecked.Value;
            }
            if (chkRetiraS.IsChecked != null)
            {
                _geraEntidades.RetiraS = chkRetiraS.IsChecked.Value;
            }
            if(chkRepositorio.IsChecked != null)
            {
                _geraEntidades.GeraRepositorio = chkRepositorio.IsChecked.Value;
            }
        }

        private void GeraOsArquivos()
        {

            ITabelasECampos tabelasECampos = TabelasECamposFactory.GetTabelasECampos(_tipoDeBanco);



            const string entidades = @"Entidades\";
            var namesPaceEntidades = _namesPace + ".Entidades";

            const string interfaces = @"Interfaces\";
            var namesPaceInterfaces = _namesPace + ".Interfaces";

            const string repositorio = @"Repositorios\";
            var namesRopositorio = _namesPace + ".Repositorios";

            const string mapeamato = @"Mapeamentos\";


            //para recuper os valores de tela tem usandar o metódo begininvoke
            //caso contrario da erro por ser um thread

            Dispatcher.BeginInvoke((UpdateUi)delegate
                                                {
                                                    pgbPrincipal.Maximum = _listacomastabelas.Count();
                                                }, null);

            var geraMapeamentos = new GeraMapeamentos(_tipoDeBanco);
            geraMapeamentos.Lazy = _lazy;
            geraMapeamentos.RetiraS = _geraEntidades.RetiraS;
            geraMapeamentos.NameSpace = _namesPace;
           
            var geraRepositorio = new GeraRepositorio();
            geraRepositorio.GeraInterace = _geraEntidades.GeraInterface;
            geraRepositorio.RetiraS = _geraEntidades.RetiraS;
            
            foreach (var tabela in _listacomastabelas)
            {

                var campos = tabelasECampos.CamposDaTabela(tabela);

             
                if (_geraEntidades.GeraInterface)
                {
                    GeraInterface(tabela, interfaces, campos, namesPaceInterfaces);
                }
             


                if (_geraEntidades.GeraClasse)
                {
                    GeraClasse(tabela, campos, entidades, namesPaceEntidades);
                    GeraXmlDeMapeamento(geraMapeamentos, tabela, mapeamato);
                }

                if (_geraEntidades.GeraRepositorio)
                {
                    GeracaoRepositorio(geraRepositorio, tabela, repositorio, namesRopositorio);
                }


              
                Dispatcher.BeginInvoke((UpdateUi)delegate
                                                    {
                                                        pgbPrincipal.Value += 1;
                                                    }, null);
            }


            
            GeraArquivoXmlDeConfiguracaoNHibernate(namesPaceEntidades);

            if (_geraEntidades.GeraRepositorio)
            {
                GeraClasseRepositorioBase(geraRepositorio, repositorio, namesRopositorio);
            }



            Dispatcher.BeginInvoke((UpdateUi)delegate
                                                {
                                                    pgbPrincipal.Value = 0;
                                                }, null);

            MessageBox.Show("Processo concluído com sucesso!", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void GeraInterface(string tabela, string interfaces, IEnumerable<string> campos, string namesPaceInterfaces)
        {
            _geraEntidades.CriaDiretorio(_caminho + interfaces);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, interfaces + "I" + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                                                   "Compile");
            _geraEntidades.EscreveNoArquivo(_caminho + interfaces + "I" + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                                            _geraEntidades.ConteudoDaInteface(campos, namesPaceInterfaces, tabela));
        }

        private void GeraClasseRepositorioBase(GeraRepositorio geraRepositorio, string repositorio, string namesRopositorio)
        {
            _geraEntidades.CriaDiretorio(_caminho + repositorio);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, repositorio + "RepositorioBase.cs",
                                                   "Compile");
            _geraEntidades.EscreveNoArquivo(_caminho + repositorio + "RepositorioBase.cs", geraRepositorio.GeraClasseRepositorioBase(namesRopositorio));
        }

        private void GeraArquivoXmlDeConfiguracaoNHibernate(string namesPaceEntidades)
        {
            var geraXmlDeConfigNHibernate = new GeraXmlDeConfigNHibernate();
            _geraEntidades.CriaDiretorio(_caminho);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, "hibernate.cfg.xml",
                                                   "Content");
            _geraEntidades.EscreveNoArquivo(_caminho + "hibernate.cfg.xml",
                                            geraXmlDeConfigNHibernate.GeraConteudoXmlDeConfigNHibernate(namesPaceEntidades, _listacomastabelas, _geraEntidades.RetiraS));
        }

        private void GeraClasse(string tabela, IEnumerable<string> campos, string entidades, string namesPaceEntidades)
        {

            _geraEntidades.CriaDiretorio(_caminho + entidades);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, entidades + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                                                   "Compile");
            _geraEntidades.EscreveNoArquivo(
                _caminho + entidades + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                _geraEntidades.ConteudoDaEntitade(campos, namesPaceEntidades, tabela));
        }

        private void GeraXmlDeMapeamento(GeraMapeamentos geraMapeamentos, string tabela, string mapeamato)
        {
            
            _geraEntidades.CriaDiretorio(_caminho + mapeamato);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, mapeamato + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".hbm.xml",
                                                   "EmbeddedResource");
            _geraEntidades.EscreveNoArquivo(
                _caminho + mapeamato + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".hbm.xml",
                geraMapeamentos.GeraMapeamentoDasClasses(tabela));
        }

        private void GeracaoRepositorio(GeraRepositorio geraRepositorio, string tabela, string repositorio, string namesRopositorio)
        {
            
            _geraEntidades.CriaDiretorio(_caminho + repositorio);
            _geraEntidades.IncluirArquivoNoProjeto(_caminhoDoArquivoProj, repositorio + "Repositorio" + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                                                   "Compile");
            _geraEntidades.EscreveNoArquivo(
                _caminho + repositorio + "Repositorio" + _trataNome.ConverteParaMaisculo(tabela, _geraEntidades.RetiraS) + ".cs",
                geraRepositorio.GeraClasseRepositorio(tabela, namesRopositorio));
        }

        private void btnListaTabelas_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ListaTabelas(true);

            }
            catch(Exception exception)
            {
                MessageBox.Show("Atenção! Erro de Conexão!\n" + exception.Message, "ERRO.", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void ListaTabelas(bool isChecked)
        {
            lsbTabelas.Items.Clear();
            var tabelasECampos = TabelasECamposFactory.GetTabelasECampos(RetornaTipoDeBancoDeDados());
            SetaValorAStringConexao();
            var tabelas = (List<string>) tabelasECampos.TabelasDoBanco();

            
            tabelas.ForEach(tabela =>
                                {
                                    var chk = new CheckBox { Content = " " + tabela, IsChecked = isChecked };
                                    lsbTabelas.Items.Add(chk);
                                    
                                });


        }

        private void ListaComaAsTabelas()
        {

            _listacomastabelas.Clear();
            // Metódo pra retorna as tabelas selecionada.

            foreach (object obj in lsbTabelas.Items)
            {
                var chk = obj as CheckBox;
                if (chk != null)
                {
                    if (chk.IsChecked != null)
                    {
                        if (chk.IsChecked.Value)
                        {
                            _listacomastabelas.Add(chk.Content.ToString().Trim());
                        }
                    }
                }
            }

        }
        private void btnNaoListaTabelas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListaTabelas(false);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Atenção! Erro de Conexão!\n" + exception.Message, "ERRO.", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void SetaValorAStringConexao()
        {
            if (chkAuthentication.IsChecked != null)
                if (!chkAuthentication.IsChecked.Value)
                {
                    StringConexao.GetSqlServer(cmbServerNameSqlServer.Text, cmbDatabaseSqlServer.Text,
                                               txtLoginSqlServer.Text, txtSenhaSqlServer.Password);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(cmbDatabaseSqlServer.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(cmbServerNameSqlServer.Text))
                        {
                            StringConexao.GetSqlServer(cmbServerNameSqlServer.Text);
                        }
                    }
                    else
                    {
                        StringConexao.GetSqlServer(cmbServerNameSqlServer.Text, cmbDatabaseSqlServer.Text);
                    }
                }
        }

        private void btnRefreshSqlServer_Click(object sender, RoutedEventArgs e)
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();
            cmbServerNameSqlServer.Items.Clear();
            DisplayData(table, cmbServerNameSqlServer);
        }

        private void DisplayData(DataTable table, ComboBox cmb)
        {
            string serviname = string.Empty;
            string instancename = string.Empty;
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    if (col.ColumnName == "ServerName")
                    {
                        serviname = "" + row[col];
                    }
                    if (col.ColumnName == "InstanceName")
                    {
                        if (!string.IsNullOrEmpty(row[col].ToString()))
                            instancename = @"\" + row[col];
                    }

                }
                cmb.Items.Add(serviname + instancename);
                serviname = string.Empty;
                instancename = string.Empty;
            }
        }

        private void cmbServerNameSqlServer_DropDownClosed(object sender, EventArgs e)
        {
            const string sql = @"sp_helpdb";


            cmbDatabaseSqlServer.Items.Clear();

            SetaValorAStringConexao();
            try
            {
                if (!string.IsNullOrWhiteSpace(cmbServerNameSqlServer.Text))
                {
                    IConexao conexao = FactoryConexao.GetConexao(RetornaTipoDeBancoDeDados());
                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            cmbDatabaseSqlServer.Items.Add(read[0].ToString());
                        }
                        read.Close();
                    }
                }
            }
            catch(Exception exception)
            {

                MessageBox.Show("Atenção! Erro de Conexão!\n" + exception.Message, "ERRO.", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private TipoDeBancoDados RetornaTipoDeBancoDeDados()
        {
            if (tbcPrincipal.SelectedIndex.ToString() == "0")
            {
                return TipoDeBancoDados.Sqlserver;
            }
            return TipoDeBancoDados.Oracle;
        }

        private void btnTestarConexao_Click(object sender, RoutedEventArgs e)
        {
            SetaValorAStringConexao();
            IConexao conexao = FactoryConexao.GetConexao(RetornaTipoDeBancoDeDados());

            if (conexao.TesteConexao())
            {
                MessageBox.Show("Conexão realizada com sucesso!", "Atenção", MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("ERRO ao realizar a conexão!", "ERRO", MessageBoxButton.OK,
                                                MessageBoxImage.Error);

            }

        }

        private void cmbDatabaseSqlServer_DropDownClosed(object sender, EventArgs e)
        {
            lsbTabelas.Items.Clear();
        }


    }
}
