using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    public class MsSql
    {
        public SqlConnection SqlConnection { get; private set; }
        public string ConnectionString { get; private set; }
        public int ConnectionTimeout { get; private set; }
        private void Connect()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            int connectionTimeout = 30;
            if (!int.TryParse(ConfigurationManager.AppSettings["ConnectionTimeout"], out connectionTimeout))
                connectionTimeout = 30;
            Connect(connectionString, connectionTimeout);
        }
        private void Connect(string connectionString, int connectionTimeout)
        {
            ConnectionString = connectionString;
            ConnectionTimeout = connectionTimeout;

            SqlConnection = new SqlConnection(connectionString);
            if (SqlConnection.State != ConnectionState.Open)
                SqlConnection.Open();
        }
        private void Close()
        {
            if (SqlConnection.State != ConnectionState.Closed)
                SqlConnection.Close();
        }
        public DataSet FillDatasetProcedure(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            DataSet mDatasetReturn = new DataSet();
            try
            {
                Connect();
                SqlCommand sqlCommad = new SqlCommand();
                sqlCommad.CommandTimeout = ConnectionTimeout;
                sqlCommad.CommandType = CommandType.StoredProcedure;
                sqlCommad.CommandText = storedProcedure;
                sqlCommad.Connection = SqlConnection;
                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCommad.Parameters.AddRange(sqlParameters.ToArray());
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCommad;
                sqlAdapter.Fill(mDatasetReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Close();
            }
            return mDatasetReturn;
        }
        //public DataSet FillDatasetProcedure(string storedProcedure)
        //{
        //    return FillDatasetProcedure(storedProcedure, null);
        //}
    }
}