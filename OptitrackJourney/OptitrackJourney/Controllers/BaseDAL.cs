using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OptitrackJourney.DAL
{
    public class BaseDAL
    {

        //declare the db connection
        protected string _connectionString;

        public BaseDAL(string connectionString)
        {
            this._connectionString = connectionString;
        }


        /// <summary>
        /// To Execute queries which returns result set (table / relation)
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sql parameters</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string query, params SqlParameter[] sqlParams)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, con);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public DataTable ExecuteMySQLDataTable(string query, bool isSp, params MySqlParameter[] sqlParams)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    cmd.Parameters.AddRange(sqlParams);
                    if (isSp)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    cmd.CommandTimeout = 200000;
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;

        }

        /// <summary>
        /// To Execute queries which returns result set (table / relation)
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sql parameters</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string query, bool isSP = false, params SqlParameter[] sqlParams)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, con);
                    if (isSP)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }


        //}
        /// <summary>
        /// To Execute queries which returns result set (table / relation)
        /// overloaded with another connection string for linkedServers
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string query, string conString = null, bool isSP = false)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conString ?? _connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, con);
                    if (isSP)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    command.CommandTimeout = 200000;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// a function which gets sql parameters and a store procedure name
        /// and excute the store procedure
        /// </summary>
        /// <param name="storeProcedureName">name of the store procedure</param>
        /// <param name="parameters">holding the store procedure parameters</param>
        public void ExecuteStoredProcedure(string storeProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(storeProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

        }

        public void ExecuteMySQLStoredProcedure(string storeProcedureName, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(storeProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
            }

        }

        /// <summary>
        /// To Execute update / insert / delete queries in Mysql
        /// </summary>
        /// <param name="query">the query string</param>
        public int ExecuteNonQueryMySql(string query, params MySqlParameter[] sqlParams)
        {
            int rowEffected = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    rowEffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rowEffected;
        }


        /// <summary>
        /// To Execute update / insert / delete queries
        /// </summary>
        /// <param name="query">the query string</param>
        public int ExecuteNonQuery(string query, params SqlParameter[] sqlParams)
        {
            int rowEffected = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    rowEffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rowEffected;
        }

        /// <summary>
        /// To Execute queries which return scalar value
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sqlParameters</param>
        /// <returns></returns>
        public object ExecuteMySqlScalar(string query, params MySqlParameter[] sqlParams)
        {
            object toRet = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    connection.Open();
                    toRet = command.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
            }
            return toRet;
        }


        /// <summary>
        /// To Execute queries which return scalar value
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sqlParameters</param>
        /// <returns></returns>
        public object ExecuteScalar(string query, params SqlParameter[] sqlParams)
        {
            object toRet = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    connection.Open();
                    toRet = command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return toRet;
        }

        /// <summary>
        /// To Execute queries which return scalar value
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sqlParameters</param>
        /// <returns></returns>
        public object ExecuteSPScalar(string query, params SqlParameter[] sqlParams)
        {
            object toRet = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    connection.Open();
                    toRet = command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return toRet;
        }



        public void LoadTableToSqlServer(DataTable SourceTable, string DestTable, string connectionString)
        {

            using (SqlConnection destCon = new SqlConnection(connectionString))
            {
                destCon.Open();

                using (SqlBulkCopy bc = new SqlBulkCopy(destCon))
                {
                    bc.BulkCopyTimeout = 5000000;
                    bc.DestinationTableName = DestTable;
                    try
                    {
                        bc.WriteToServer(SourceTable);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }

        }


        public void LoadTableToSqlServer(DataTable SourceTable, string DestTable)
        {

            using (SqlConnection destCon = new SqlConnection(_connectionString))
            {
                destCon.Open();

                using (SqlBulkCopy bc = new SqlBulkCopy(destCon))
                {
                    bc.BulkCopyTimeout = 5000000;
                    bc.DestinationTableName = DestTable;
                    try
                    {
                        bc.WriteToServer(SourceTable);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }

        }

        /// <summary>
        /// To Execute update / insert / delete queries
        /// </summary>
        /// <param name="query">the query string</param>
        public int MySqlExecuteNonQuery(string query)
        {
            int rowEffected = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    rowEffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rowEffected;
        }

        /// <summary>
        /// To Execute queries which return scalar value
        /// </summary>
        /// <param name="query">the query string</param>
        /// <param name="sqlParams">sqlParameters</param>
        /// <returns></returns>
        public object MySqlExecuteScalar(string query, bool isSp = false, params MySqlParameter[] sqlParams)
        {
            object toRet = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.CommandTimeout = 200000;
                    command.Parameters.AddRange(sqlParams);
                    if (isSp)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    connection.Open();
                    toRet = command.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
            }
            return toRet;
        }
        public DataSet GetDataSet(string sql, params MySqlParameter[] parameters)
        {
            return GetDataSet(sql, CommandType.StoredProcedure, parameters);
        }

        public DataSet GetDataSet(string sql, CommandType commandType, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        DataSet dt = new DataSet();
                        command.CommandType = commandType;
                        command.CommandTimeout = 200000;
                        command.Parameters.AddRange(parameters);
                        connection.Open();
                        MySqlDataAdapter ad = new MySqlDataAdapter();
                        ad.SelectCommand = command;
                        ad.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception cought at DAL.GetData: " + ex.Message);
            }
        }

    }
}