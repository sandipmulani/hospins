using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace hospins.Repository.DataServices
{
    public class DatabaseHelper : IDisposable
    {
        private readonly SqlConnection objConnection;

        public static string ConnectionString { get; set; }
        public SqlBulkCopy bulkCopy;
        public bool HasError { get; set; } = false;

        /*public DatabaseHelper()
        {
            
            //objConnection = new SqlConnection(@"data source=192.168.9.113,1433;initial catalog=InchCapeDB;persist security info=True;user id=sa;password=CDI.1234;");
            objConnection = new SqlConnection(ConfigurationSettings.ConnectionStrings.Value.strDBConn.ToString());
            objCommand = new SqlCommand();
            objCommand.CommandTimeout = 120;
            objCommand.Connection = objConnection;
        }
        public DatabaseHelper(bool IsBulCopy = false, bool IsKeepIdentityBulCopy = false)
        {
            objConnection = new SqlConnection(ConfigurationSettings.ConnectionStrings.Value.strDBConn.ToString());
            objCommand = new SqlCommand();
            objCommand.CommandTimeout = 120;
            objCommand.Connection = objConnection;
            if (IsBulCopy)
            {
                if (IsKeepIdentityBulCopy)
                {
                    bulkCopy = new SqlBulkCopy(objConnection,SqlBulkCopyOptions.KeepIdentity,null);
                }
                else
                {
                    bulkCopy = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.UseInternalTransaction,null);
                }
            }
        }*/
        public static void SetConnectionString(string connectionString)
        {
            if (ConnectionString == null)
            {
                ConnectionString = connectionString;
            }
            else
            {
                throw new Exception();
            }
        }

        public DatabaseHelper()
        {
            objConnection = new SqlConnection(ConnectionString);
            Command = new SqlCommand
            {
                CommandTimeout = 120,
                Connection = objConnection
            };
        }

        public DatabaseHelper(string strDBConn, bool IsBulCopy = false, bool IsKeepIdentityBulCopy = false)
        {
            objConnection = new SqlConnection(strDBConn);
            Command = new SqlCommand
            {
                CommandTimeout = 120,
                Connection = objConnection
            };
            if (IsBulCopy)
            {
                if (IsKeepIdentityBulCopy)
                {
                    bulkCopy = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.KeepIdentity, null);
                }
                else
                {
                    bulkCopy = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.UseInternalTransaction, null);
                }
            }
        }

        public SqlParameter AddParameter(string name, object value)
        {
            SqlParameter p = Command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            if ((p.SqlDbType == SqlDbType.VarChar) || (p.SqlDbType == SqlDbType.NVarChar))
            {
                p.Size = (p.SqlDbType == SqlDbType.VarChar) ? 8000 : 4000;

                if ((value != null) && !(value is DBNull) && (value.ToString().Length > p.Size))
                    p.Size = -1;
            }
            return Command.Parameters.Add(p);
        }

        public SqlParameter AddParameter(string name, object value, ParamType type)
        {
            SqlParameter p = Command.CreateParameter();
            if (type == ParamType.Output)
                p.Direction = ParameterDirection.Output;
            p.ParameterName = name;
            p.Value = value;
            if ((p.SqlDbType == SqlDbType.VarChar) || (p.SqlDbType == SqlDbType.NVarChar))
            {
                p.Size = (p.SqlDbType == SqlDbType.VarChar) ? 8000 : 4000;

                if ((value != null) && !(value is DBNull) && (value.ToString().Length > p.Size))
                    p.Size = -1;
            }
            return Command.Parameters.Add(p);
        }

        public SqlParameter AddParameter(SqlParameter parameter)
        {
            return Command.Parameters.Add(parameter);
        }

        public void ClearParameters()
        {
            Command.Parameters.Clear();
        }

        public SqlCommand Command { get; }

        public bool ExecuteBulkCopy(DataTable dt)
        {
            bool result = false;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }

                bulkCopy.BulkCopyTimeout = 0;

                bulkCopy.WriteToServer(dt);

                result = true;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, "ExecuteBulkCopy");
            }
            finally
            {
                bulkCopy.Close();

                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return result;
        }

        public void BeginTransaction()
        {
            if (objConnection.State == System.Data.ConnectionState.Closed)
            {
                objConnection.Open();
            }
            Command.Transaction = objConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Command.Transaction.Commit();
            objConnection.Close();
        }

        public void RollbackTransaction()
        {
            Command.Transaction.Rollback();
            objConnection.Close();
        }

        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, CommandType.Text);
        }

        public int ExecuteNonQuery(string query, CommandType commandtype)
        {
            Command.CommandText = query;
            Command.CommandType = commandtype;

            int i = -1;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SqlException sx = (SqlException)ex;
                if (sx.Number == 547)       // Foreign Key Error
                    i = sx.Number;
                else
                    HandleExceptions(ex, query);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return i;
        }

        public int ExecuteNonQuery(string spName, SqlParameter[] para)
        {
            Command.CommandText = spName;
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddRange(para);
            int i = -1;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SqlException sx = (SqlException)ex;
                if (sx.Number == 547)       // Foreign Key Error
                    i = sx.Number;
                else
                    HandleExceptions(ex, spName);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return i;
        }

        public string ExecuteNonQuery(string query, CommandType commandtype, string outputparam)
        {
            Command.CommandText = query;
            Command.CommandType = commandtype;
            string outputParamValue = "";
            int i;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = Command.ExecuteNonQuery();
                outputParamValue = Command.Parameters[outputparam].Value.ToString();
            }
            catch (Exception ex)
            {
                SqlException sx = (SqlException)ex;
                if (sx.Number == 547)       // Foreign Key Error
                {
                    i = sx.Number;
                }
                else
                {
                    Command.Transaction.Rollback();
                    HandleExceptions(ex, query);
                }
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return outputParamValue;
        }

        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, CommandType.Text);
        }

        public object ExecuteScalar(string query, CommandType commandtype)
        {
            Command.CommandText = query;
            Command.CommandType = commandtype;
            object o = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                o = Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, query);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return o;
        }

        public SqlDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text);
        }

        public SqlDataReader ExecuteReader(string query, CommandType commandtype)
        {
            Command.CommandText = query;
            Command.CommandType = commandtype;
            SqlDataReader reader = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                reader = Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, query);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return reader;
        }

        public DataSet ExecuteDataSet(string query)
        {
            return ExecuteDataSet(query, CommandType.Text);
        }

        public DataSet ExecuteDataSet(string query, CommandType commandtype)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            Command.CommandText = query;
            Command.CommandType = commandtype;
            adapter.SelectCommand = Command;
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, query);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return ds;
        }

        public DataTable FetchDataTableBySP(string Spname, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddRange(para);
            Command.CommandText = Spname;
            SqlDataAdapter da = new SqlDataAdapter(Command);
            try
            {
                objConnection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, Spname);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return dt;
        }

        public DataTable FetchDataTableByQuery(string Qry, SqlParameter[] para = null)
        {
            DataTable dt = new DataTable();
            Command.CommandType = CommandType.Text;
            if (para != null)
                Command.Parameters.AddRange(para);
            Command.CommandText = Qry;
            SqlDataAdapter da = new SqlDataAdapter(Command);
            try
            {
                objConnection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, Qry);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return dt;
        }

        public List<T> FetchListBySP<T>(string Spname, SqlParameter[] para)
        {
            List<T> lst = new List<T>();
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddRange(para);
            Command.CommandText = Spname;
            try
            {
                objConnection.Open();
                var dr = Command.ExecuteReader();
                while (dr.Read())
                {
                    T item = CommonFunctions.GetListItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, Spname);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return lst;
        }

        public List<T> FetchListByQuery<T>(string Qry, SqlParameter[] para = null)
        {
            List<T> lst = new List<T>();
            Command.CommandType = CommandType.Text;
            Command.Parameters.AddRange(para);
            Command.CommandText = Qry;
            try
            {
                objConnection.Open();
                var dr = Command.ExecuteReader();
                while (dr.Read())
                {
                    T item = CommonFunctions.GetListItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, Qry);
            }
            finally
            {
                //objCommand.Parameters.Clear();
                if (Command.Transaction == null)
                {
                    objConnection.Close();
                }
            }
            return lst;
        }

        private void HandleExceptions(Exception ex, string query)
        {
            HasError = true;
            //Dispose(); //Possible same helper use for diff query so not disposed
            throw new DatabaseHelperException(String.Format("Query : {0}", query), ex);
            //WriteToLog(ex.Message, query);
        }

        /*private void WriteToLog(string msg, string query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (msg == null)
                throw new ArgumentNullException(nameof(msg));

            try
            {
                /*string strLogFile = CurrentContext.Config.UploadPath + "Logfile.txt";
                if (strLogFile != "")
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(strLogFile);
                    writer.WriteLine("Date and Time : " + DateTime.Now.ToString() + " - " + msg);
                    writer.WriteLine("Error in Query : " + query);
                    writer.WriteLine("");
                    writer.Close();
                }* /
            }
            catch { }
        }*/

        public void Dispose()
        {
            Command.Parameters.Clear();
            objConnection.Close();
            objConnection.Dispose();
            Command.Dispose();
        }

        public enum ParamType
        {
            Input, Output, InputOutput
        }
    }

    public class DatabaseHelperException : Exception
    {
        public DatabaseHelperException() { }

        public DatabaseHelperException(string message, Exception inner)
        : base(message, inner) { }

        public DatabaseHelperException(string message) : base(message)
        {
        }
    }
}

