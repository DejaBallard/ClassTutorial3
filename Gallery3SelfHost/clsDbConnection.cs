using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Configuration;

namespace Gallery3SelfHost
{
    /// <summary>
    /// This is a standard Database connection class that was supplied to us by the tutor
    /// </summary>
    public class clsDbConnection
    {

        private static ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings["GalleryDatabase"];

        private static DbProviderFactory ProviderFactory = DbProviderFactories.GetFactory(ConnectionStringSettings.ProviderName);

        private static string ConnectionStr = ConnectionStringSettings.ConnectionString;

        /// <summary>
        /// Gets the data from the database tableS
        /// </summary>
        /// <param name="prSQL">SQL statement that is being used, without data</param>
        /// <param name="prPars">Data that needs to be inserted into the SQL statement</param>
        /// <returns>table with data</returns>
        public static DataTable GetDataTable(string prSQL, Dictionary<string, Object> prPars)
        {

                using (DataTable lcDataTable = new DataTable("TheTable"))
                using (DbConnection lcDataConnection = ProviderFactory.CreateConnection())
                using (DbCommand lcCommand = lcDataConnection.CreateCommand())
                {
                    lcDataConnection.ConnectionString = ConnectionStr;
                    lcDataConnection.Open();
                    lcCommand.CommandText = prSQL;
                    setPars(lcCommand, prPars);
                    using (DbDataReader lcDataReader = lcCommand.ExecuteReader(CommandBehavior.CloseConnection))
                        lcDataTable.Load(lcDataReader);
                    return lcDataTable;
                }

        }

        /// <summary>
        /// Execute a SQL statement, without the need of returning data, just the result of it working
        /// </summary>
        /// <param name="prSQL">SQL statement without the data</param>
        /// <param name="prPars">Data that needs to be inserted into the SQL statement</param>
        /// <returns>The amount of rows effected</returns>
        public static int Execute(string prSQL, Dictionary<string, Object> prPars)
        {
            using (DbConnection lcDataConnection = ProviderFactory.CreateConnection())
            using (DbCommand lcCommand = lcDataConnection.CreateCommand())
            {
                lcDataConnection.ConnectionString = ConnectionStr;
                lcDataConnection.Open();
                lcCommand.CommandText = prSQL;
                setPars(lcCommand, prPars);
                return lcCommand.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Not 100% sure
        /// </summary>
        /// <param name="prCommand"></param>
        /// <param name="prPars"></param>
        private static void setPars(DbCommand prCommand, Dictionary<string, Object> prPars)
        { // For most DBMS using @Name1, @Name2, @Name3 etc.
            if (prPars != null)
                foreach (KeyValuePair<string, Object> lcItem in prPars)
                {
                    DbParameter lcPar = ProviderFactory.CreateParameter();
                    lcPar.Value = lcItem.Value == null ? DBNull.Value : lcItem.Value;
                    lcPar.ParameterName = '@' + lcItem.Key;
                    prCommand.Parameters.Add(lcPar);
                }
        }
    }
    }

