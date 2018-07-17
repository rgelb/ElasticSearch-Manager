using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ElasticSearchManager
{
    public class Database : IDisposable
    {
        SqlTransaction _Transaction = null;
        private const int CMD_TIMEOUT = 120;

        public void BeginTransaction()
        {
            _Transaction = new SqlConnection().BeginTransaction();


        }

        public void CommitTransaction()
        {
            if (_Transaction == null)
                throw new Exception("Transaction has started, thus it cannot be committed");

            _Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (_Transaction == null)
                throw new Exception("Transaction has started, thus it cannot be rolled back");

            _Transaction.Rollback();

        }

        /// <summary>
        /// Attempts the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        protected bool AttemptConnection(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string connectionString, string commandText)
        {
            return ExecuteDataTable(connectionString, commandText, null);
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string connectionString, string commandText, IEnumerable<DbParameter> parameters)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                param.SynchronizeOutputValue();
                        }

                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the data set. This can be used when multiple result sets are expected.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>Dataset</returns>
        protected DataSet ExecuteDataSet(string connectionString, string commandText)
        {
            return ExecuteDataSet(connectionString, commandText, null);
        }

        /// <summary>
        /// Executes the data set. This can be used when multiple result sets are expected.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Dataset</returns>
        protected DataSet ExecuteDataSet(string connectionString, string commandText, IEnumerable<DbParameter> parameters)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);

                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                param.SynchronizeOutputValue();
                        }

                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected DataTable ExecuteDataTableWithTrans(string connectionString, string commandText, IEnumerable<DbParameter> parameters)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        try
                        {
                            cmd.Transaction = conn.BeginTransaction();
                            da.Fill(dt);
                            cmd.Transaction.Commit();

                            if (parameters != null)
                            {
                                foreach (var param in parameters)
                                    param.SynchronizeOutputValue();
                            }

                            return dt;
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (cmd.Transaction != null)
                                    cmd.Transaction.Rollback();
                            }
                            catch (Exception) { }
                            throw ex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        protected object ExecuteScalar(string connectionString, string commandText)
        {
            return ExecuteScalar(connectionString, commandText, null);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns></returns>
        protected object ExecuteScalar(string connectionString, string commandText, IEnumerable<DbParameter> parameters)
        {
            return ExecuteScalar(connectionString, commandText, parameters, CMD_TIMEOUT);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns></returns>
        protected object ExecuteScalar(string connectionString, string commandText, IEnumerable<DbParameter> parameters, int timeout)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Only use the parameterized timeout value if it's larger than 0
                    cmd.CommandTimeout = timeout > 0 ? timeout : CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    object result = cmd.ExecuteScalar();

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            param.SynchronizeOutputValue();
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string connectionString, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandText, null);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string connectionString, string commandText, IEnumerable<DbParameter> parameters)
        {
            return ExecuteNonQuery(connectionString, commandText, parameters, CMD_TIMEOUT);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string connectionString, string commandText, IEnumerable<DbParameter> parameters, int timeout)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Only use the parameterized timeout value if it's larger than 0
                    cmd.CommandTimeout = timeout > 0 ? timeout : CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    int result = cmd.ExecuteNonQuery();

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            param.SynchronizeOutputValue();
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Executes the non query text.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        protected int ExecuteNonQueryText(string connectionString, string commandText)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes the data table text.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        protected DataTable ExecuteDataTableText(string connectionString, string commandText)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public delegate void DataReaderDelegate(IDataReader reader);
        protected void ExecuteDataReader(string connectionString, string commandText, IEnumerable<DbParameter> parameters, DataReaderDelegate del)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CMD_TIMEOUT;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param.CreateSqlParameter());
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    try
                    {
                        del(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
        }


        /// <summary>
        /// Gets the query file path.
        /// </summary>
        /// <param name="fileTitle">The file title.</param>
        /// <returns></returns>
        protected string GetQueryFilePath(string fileTitle)
        {
            string fullPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var fi = new FileInfo(fullPath);
            string queriesFolder = Path.Combine(fi.DirectoryName, "Queries");
            return Path.Combine(queriesFolder, fileTitle);
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (_Transaction != null)
            {
                _Transaction.Dispose();
                _Transaction = null;
            }
        }

        #endregion
    }

    public class DbParameter
    {
        /// <summary>
        /// Name of stored proc parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value to assign to the parameter.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Indicates whether the parameter should contain an output value from the stored proc.
        /// </summary>
        public bool Output { get; set; }

        /// <summary>
        /// Reference to the SqlParameter that was created for this DbParameter
        /// </summary>
        public SqlParameter SqlParam { get; set; }

        public DbParameter(string name, object value)
            : this(name, value, false)
        {
        }

        public DbParameter(string name, object value, bool output)
        {
            Name = name;
            Value = value;
            Output = output;
        }

        /// <summary>
        /// Creates a SqlParameter object to pass to SqlCommand object.
        /// </summary>
        /// <returns>SqlParameter object</returns>
        public SqlParameter CreateSqlParameter()
        {
            SqlParam = new SqlParameter(this.Name, this.Value);
            if (this.Output)
                SqlParam.Direction = ParameterDirection.Output;

            return SqlParam;
        }

        /// <summary>
        /// Upates value of output SqlParameter to DbParameter object.
        /// </summary>
        public void SynchronizeOutputValue()
        {
            if (Output && SqlParam != null)
                Value = SqlParam.Value;
        }
    }
}
