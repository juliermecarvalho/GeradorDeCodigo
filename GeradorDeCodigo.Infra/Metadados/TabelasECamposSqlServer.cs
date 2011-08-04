using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using GeradorDeCodigo.Infra.Conexao;
using GeradorDeCodigo.Infra.Enun;

namespace GeradorDeCodigo.Infra.Metadados
{
    public class TabelasECamposSqlServer : ITabelasECampos
    {


        private readonly TipoDeBancoDados _tipoDeBancoDados;

        public TabelasECamposSqlServer()
        {
            _tipoDeBancoDados = TipoDeBancoDados.Sqlserver;
        }


        public bool VerificaSeExisteTabela(string tabela)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(tabela));

            using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
            {

                const string sql =
                    @"SELECT  SO.NAME AS TABELA
                            FROM SYSOBJECTS SO
                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                            WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                AND SO.NAME = @NAME";
                string toReturn = string.Empty;

                using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                {
                    select.Parameters.AddWithValue("@NAME", tabela);
                    var read = select.ExecuteReader();
                    while (read.Read())
                    {
                        toReturn = read["TABELA"].ToString().ToUpper();
                    }
                    read.Close();

                }
                return !string.IsNullOrWhiteSpace(toReturn);
            }

        }

        public IEnumerable<string> TabelasDoBanco()
        {
            using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
            {
                const string sql = @"SELECT  DISTINCT(SO.NAME) AS TABELA	
                                            FROM SYSOBJECTS SO
                                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                            WHERE SO.XTYPE = 'U' 
                                              AND SO.NAME <> 'DTPROPERTIES'
                                              AND SO.NAME <> 'SYSDIAGRAMS'
                                              ORDER BY SO.NAME";

                var camposDaTabela = new List<string>();

                using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                {
                    var read = select.ExecuteReader();

                    while (read.Read())
                    {
                        camposDaTabela.Add(read["TABELA"].ToString().ToUpper());
                    }
                    read.Close();
                }
                return camposDaTabela;
            }

        }


        public string TamanhodoCamposString(string tabela, string campo)
        {

            Contract.Requires(!string.IsNullOrWhiteSpace(campo));

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    var toReturn = string.Empty;
                    const string sql =
                        @"SELECT  DISTINCT(SC.NAME) AS CAMPO, SC.LENGTH
	                                FROM SYSOBJECTS SO
		                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
		                                LEFT JOIN SYSTYPES ST ON SC.XTYPE = ST.XTYPE
		                                LEFT JOIN SYSCOMMENTS SM ON SC.CDEFAULT = SM.ID
		                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
	                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
									AND SO.NAME = @COLUMN_NAME 
									AND SC.NAME = @TABLE_NAME";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@COLUMN_NAME", tabela);
                        select.Parameters.AddWithValue("@TABLE_NAME", campo);
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            toReturn = read["LENGTH"].ToString();
                        }

                        if (!string.IsNullOrWhiteSpace(toReturn))
                        {
                            var i = int.Parse(toReturn);
                            i = i / 2;
                            toReturn = i.ToString();
                        }
                        return toReturn;

                    }

                }
            }
            return string.Empty;


        }

        public IEnumerable<string> CamposDaTabela(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {

                    const string sql =
                        @"SELECT  DISTINCT(SC.NAME) AS CAMPO, SC.COLID
	                                FROM SYSOBJECTS SO
		                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
		                                LEFT JOIN SYSTYPES ST ON SC.XTYPE = ST.XTYPE
		                                LEFT JOIN SYSCOMMENTS SM ON SC.CDEFAULT = SM.ID
		                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
	                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
	                                  AND SO.NAME = @NAME
                                      ORDER BY SC.COLID";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@NAME", tabela);
                        var camposDaTabela = new List<string>();
                        var read = select.ExecuteReader();
                        string teste = string.Empty;
                        while (read.Read())
                        {
                            camposDaTabela.Add(read["CAMPO"].ToString().ToUpper());
                        }
                        read.Close();

                        return camposDaTabela;
                    }
                }
            }
            return null;

        }

        public IEnumerable<string> CamposChavesDaTabela(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    string sql = string.Format(
                        @"SELECT COLUMN_NAME
                                FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'ISIDENTITY') = 1
                                
                                AND TABLE_NAME = '{0}'", tabela);

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        
                        var camposDaTabela = new List<string>();
                        var read = select.ExecuteReader();

                        while (read.Read())
                        {
                            camposDaTabela.Add(read["COLUMN_NAME"].ToString().ToUpper());
                        }
                        read.Close();

                        return camposDaTabela;
                    }
                }
            }
            return null;

        }

        public bool CampoChavesDaTabela(string tabela, string campo)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(campo));
            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {


                    const string sql =
                        @"SELECT   SC.NAME AS CAMPO
	                                FROM SYSOBJECTS SO
		                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
		                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
	                                WHERE SO.XTYPE = 'U' 
		                                AND CPK.TABLE_NAME = @TABLE_NAME
										AND SC.NAME = @CAMPO
		                                AND (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISPRIMARYKEY')) = 1";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);
                        select.Parameters.AddWithValue("@CAMPO", campo);
                        var read = select.ExecuteReader();
                        bool toReturn = false;
                        while (read.Read())
                        {
                            toReturn = (read["CAMPO"].ToString().ToUpper()) != string.Empty;
                        }
                        read.Close();

                        return toReturn;
                    }

                }
            }
            return false;

        }

        public string DialectDoBancodeDados()
        {
            using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
            {
                const string sql = @"SELECT @@VERSION";

                using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                {
                    var read = select.ExecuteReader();
                    string toReturn = string.Empty;
                    while (read.Read())
                    {
                        toReturn = (read[0].ToString().ToUpper());
                    }
                    read.Close();


                    if (toReturn.Substring(22, 4) == "2000")
                    {
                        toReturn = "2000";
                    }
                    else if (toReturn.Substring(21, 4) == "2005")
                    {
                        toReturn = "2005";
                    }
                    else if (toReturn.Substring(21, 4) == "2008")
                    {
                        toReturn = "2008";
                    }
                    else
                    {
                        toReturn = "2005";
                    }

                    toReturn = "NHibernate.Dialect.MsSql" + toReturn + "Dialect";
                    return toReturn;
                }
            }
        }

        public string ConnectionDriver()
        {
            return "NHibernate.Driver.SqlClientDriver";
        }

        public string CampoChavesDaTabela(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {

                    const string sql =
                        @"SELECT   SC.NAME AS CAMPO
	                                FROM SYSOBJECTS SO
		                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
		                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
	                                WHERE SO.XTYPE = 'U' 
		                                AND CPK.TABLE_NAME = @TABLE_NAME
		                                AND (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISPRIMARYKEY')) = 1";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);
                        var read = select.ExecuteReader();
                        string toReturn = string.Empty;
                        while (read.Read())
                        {
                            toReturn = (read["CAMPO"].ToString().ToUpper());
                        }
                        read.Close();

                        return toReturn;
                    }
                }
            }
            return null;

        }

        public bool CampoOBrigatorio(string tabela, string campo)
        {

            Contract.Requires(!string.IsNullOrWhiteSpace(campo));

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    var toReturn = false;
                    const string sql =
                        @"SELECT  SC.ISNULLABLE
	                                FROM SYSOBJECTS SO
		                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
		                                LEFT JOIN SYSTYPES ST ON SC.XTYPE = ST.XTYPE
		                                LEFT JOIN SYSCOMMENTS SM ON SC.CDEFAULT = SM.ID
		                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
	                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
	                                  AND SO.NAME = @TABLE_NAME
									  AND SC.NAME = @COLUMN_NAME";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@COLUMN_NAME", campo);
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            toReturn = int.Parse(read["ISNULLABLE"].ToString()) == 0;
                        }

                        return toReturn;
                    }

                }
            }
            return false;

        }

        public IEnumerable<string> TabelasOndeARelacionamento(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    var toReturn = new List<string>();
                    const string sql =
                        @"SELECT (SO.NAME)
                                FROM SYSOBJECTS SO
                                    LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                      AND  (SELECT  
                                             TABLE_PK =(SELECT B.TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE B 
                                             WHERE B.CONSTRAINT_NAME = A.UNIQUE_CONSTRAINT_NAME)
		                                    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A 
                                            WHERE CONSTRAINT_NAME = CPK.CONSTRAINT_NAME) = @TABLE_NAME";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            toReturn.Add(read["NAME"].ToString());
                        }

                        return toReturn;
                    }

                }
            }
            return null;

        }

        public string NomeDoCampoOndeARelacionmento(string tabela, string fkTabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    var toReturn = string.Empty;
                    const string sql =
                        @"SELECT U.TABELA, U.CAMPO, U.FK_TABELA FROM (SELECT SC.COLORDER AS POSICAO, SO.NAME AS TABELA, SC.NAME AS CAMPO,
                                            ST.NAME AS TIPO, SC.LENGTH AS TAMANHO, SM.TEXT AS VALOR_PADRAO,
                                                CASE SC.ISNULLABLE
                                                    WHEN 0 THEN 'NOT_NULL'
                                                ELSE 'NULL' END AS NOT_NULL,
                                                CASE 
                                                    WHEN (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISPRIMARYKEY')) = 1 THEN 'PK'
                                                    WHEN (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISFOREIGNKEY')) = 1 THEN 'FK'
                                                ELSE 'NULL' END AS 'KEY',
                                                CASE 
                                                    WHEN (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISFOREIGNKEY')) = 1 THEN 
                                                    (SELECT  
                                                         TABLE_PK =(SELECT B.TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE B 
                                                         WHERE B.CONSTRAINT_NAME = A.UNIQUE_CONSTRAINT_NAME )
                                                     FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A WHERE CONSTRAINT_NAME = CPK.CONSTRAINT_NAME)

                                                ELSE 'NULL' END AS FK_TABELA
                                            FROM SYSOBJECTS SO
                                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                                LEFT JOIN SYSTYPES ST ON SC.XTYPE = ST.XTYPE
                                                LEFT JOIN SYSCOMMENTS SM ON SC.CDEFAULT = SM.ID
                                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                            WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                            AND SO.NAME = @TABELA)U WHERE U.FK_TABELA = @FK_TABELA";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@TABELA", tabela);
                        select.Parameters.AddWithValue("@FK_TABELA", fkTabela);
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            toReturn = read["CAMPO"].ToString();
                        }

                        return toReturn;
                    }

                }

            }
            return null;

        }

        public IEnumerable<string> TabelasOndeARelacionamentoCamposRepetidos(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    var toReturn = new List<string>();
                    const string sql =
                        @"SELECT  DISTINCT(CAMPOS_REPETIODS), NAME 
                                    FROM
                                    (
                                    SELECT (SO.NAME) +' ' + (CPK.COLUMN_NAME) AS CAMPOS_REPETIODS, SO.NAME AS NAME
                                                FROM SYSOBJECTS SO
                                                    LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                                    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                                      AND  (SELECT  
                                                             TABLE_PK =(SELECT B.TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE B 
                                                             WHERE B.CONSTRAINT_NAME = A.UNIQUE_CONSTRAINT_NAME)
                                                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A 
                                                            WHERE CONSTRAINT_NAME = CPK.CONSTRAINT_NAME) = @TABLE_NAME
                                    )Z WHERE 
                                    (
                                    SELECT COUNT (SO.NAME) AS NAME
                                                FROM SYSOBJECTS SO
                                                    LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                                    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                                      AND  (SELECT  
                                                             TABLE_PK =(SELECT B.TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE B 
                                                             WHERE B.CONSTRAINT_NAME = A.UNIQUE_CONSTRAINT_NAME)
                                                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A 
                                                            WHERE CONSTRAINT_NAME = CPK.CONSTRAINT_NAME) = @TABLE_NAME
						                                    AND SO.NAME = Z.NAME
                                    ) > 1";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);
                        var read = select.ExecuteReader();
                        while (read.Read())
                        {
                            toReturn.Add(read["CAMPOS_REPETIODS"].ToString());
                        }

                        return toReturn;
                    }

                }
            }
            return null;
            
        }



        public bool CamposIdentityDaTabela(string tabela, string campo)
        {

            Contract.Requires(!string.IsNullOrWhiteSpace(campo));
            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {


                    const string sql =
                        @"SELECT COLUMN_NAME
                                FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'ISIDENTITY') = 1

                                AND TABLE_NAME = @TABLE_NAME";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        
                        select.Parameters.AddWithValue("@TABLE_NAME", tabela);

                        var read = select.ExecuteReader();
                        var toReturn = false;
                        while (read.Read())
                        {
                            toReturn = read["COLUMN_NAME"].ToString().ToUpper() == campo.ToUpper();
                        }

                        return toReturn;
                    }

                }
            }
            return false;
            

        }

        public IEnumerable<string> CamposChavesEstrageiraDaTabela(string tabela)
        {

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    const string sql =
                        @"SELECT  SC.NAME AS CAMPO
                                FROM SYSOBJECTS SO
                                    LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                    AND SO.NAME = @NAME
                                    AND (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISFOREIGNKEY')) = 1";

                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@NAME", tabela);
                        var camposDaTabela = new List<string>();
                        var read = select.ExecuteReader();

                        while (read.Read())
                        {
                            camposDaTabela.Add(read["CAMPO"].ToString().ToUpper());

                        }
                        read.Close();

                        return camposDaTabela;
                    }
                }
            }
            return null;
        }

        public string NomeDaTabelaDaChaveEstrgeira(string tabela, string campo)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(campo));

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    const string sql =
                        @"SELECT * FROM(
                                    SELECT 	                            
							        CASE 
                                    WHEN (SELECT OBJECTPROPERTY(OBJECT_ID(CPK.CONSTRAINT_NAME), 'ISFOREIGNKEY')) = 1 THEN 
                                    (SELECT  
	                                     TABLE_PK =(SELECT B.TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE B 
                                         WHERE B.CONSTRAINT_NAME = A.UNIQUE_CONSTRAINT_NAME)
                                     FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A WHERE CONSTRAINT_NAME = CPK.CONSTRAINT_NAME)

                                ELSE 'NULL' END AS FK_TABELA
                            FROM SYSOBJECTS SO
                                LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                            WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
							    AND SO.NAME = @TABELA
							    AND SC.NAME = @CAMPO ) U WHERE U.FK_TABELA <> 'NULL'";


                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@CAMPO", campo);
                        select.Parameters.AddWithValue("@TABELA", tabela);
                        var read = select.ExecuteReader();
                        string tipoDeCampoDaTabela = string.Empty;
                        while (read.Read())
                        {
                            tipoDeCampoDaTabela = read["FK_TABELA"].ToString().ToUpper();
                        }

                        read.Close();

                        return tipoDeCampoDaTabela;
                    }

                }
            }
            return string.Empty;
        }

        public string TipoDadoBd(string tabela, string campo)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(campo));

            if (VerificaSeExisteTabela(tabela))
            {
                using (var conexao = FactoryConexao.GetConexao(_tipoDeBancoDados))
                {
                    const string sql =
                        @"SELECT   DISTINCT(ST.NAME) AS TIPO
                                    FROM SYSOBJECTS SO
                                        LEFT JOIN SYSCOLUMNS SC ON SO.ID = SC.ID
                                        LEFT JOIN SYSTYPES ST ON SC.XTYPE = ST.XTYPE		                                
                                        LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CPK ON CPK.TABLE_NAME = SO.NAME AND CPK.COLUMN_NAME = SC.NAME
                                    WHERE SO.XTYPE = 'U' AND SO.NAME <> 'DTPROPERTIES'
                                        AND ST.NAME <> 'sysname'
						            AND SO.NAME = @TABELA
						            AND SC.NAME = @CAMPO";


                    using (var select = new SqlCommand(sql, conexao.Connection<SqlConnection>()))
                    {
                        select.Parameters.AddWithValue("@CAMPO", campo);
                        select.Parameters.AddWithValue("@TABELA", tabela);
                        var read = select.ExecuteReader();
                        var tipoDeCampoDaTabela = string.Empty;
                        while (read.Read())
                        {
                            tipoDeCampoDaTabela = read["TIPO"].ToString().ToUpper();
                        }
                        read.Close();

                        return tipoDeCampoDaTabela;
                    }

                }
            }
            return string.Empty;
        }


    }


}
