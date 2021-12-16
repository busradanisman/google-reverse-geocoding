using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    class ThreadInformation
    {
        public int Id { get; set; }
        public MsSql MsSql { get; private set; }
        public GoogleGeocodingResult GeocodingResult { get; set; }

        public static bool _ServiceIsAlive = true;
        public ThreadInformation(int id)
        {
            Id = id;
            this.MsSql = new MsSql();
            this.GeocodingResult = new GoogleGeocodingResult();
        }
        public void GetCoordinateListFromDatabase()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "@id", Value = GeocodingResult.ReturnId });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@ThreadId", Value = Id });

            DataSet mDataSetCoordinateList = MsSql.FillDatasetProcedure(ConfigurationManager.AppSettings["SelectCoordinateFromDatabase"], sqlParameters);

            foreach (DataRow dataRowCoordinate in mDataSetCoordinateList.Tables[0].Rows)
            {
                GeocodingResult.ReturnId = dataRowCoordinate["id"].ToString();
                GeocodingResult.ReturnLat = dataRowCoordinate["Lat"].ToString();
                GeocodingResult.ReturnLng = dataRowCoordinate["Lng"].ToString();

                GeocodingResult.GetAdressFromLatLng();
                SaveCoordinateResultToDatabase();

                if (mDataSetCoordinateList.Tables.Count != 0)
                    Logger.Trace("Insert Successful" + Environment.NewLine + GeocodingResult.ReturnLat + "\t" + GeocodingResult.ReturnLng + " \tLat and Lng Coordinate Added To The Completed Table" + Environment.NewLine);
            }
        }
        public void SaveCoordinateResultToDatabase()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "@id", Value = GeocodingResult.ReturnId });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@ThreadId", Value = Id });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@Lat", Value = GeocodingResult.ReturnLat });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@Lng", Value = GeocodingResult.ReturnLng });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@Address", Value = GeocodingResult.FormattedAddress });
            sqlParameters.Add(new SqlParameter() { ParameterName = "@Thread_Name", Value = Thread.CurrentThread.Name });

            DataSet mDataSetResult = MsSql.FillDatasetProcedure(ConfigurationManager.AppSettings["InsertCoordinateFromDatabase"], sqlParameters);
        }
        public void DoOperation()
        {
            while (_ServiceIsAlive)
            {
                try
                {
                    GetCoordinateListFromDatabase();

                    int threadSleepFreq;
                    if (!int.TryParse(ConfigurationManager.AppSettings["ThreadSleepFreq"], out threadSleepFreq))
                    {
                        threadSleepFreq = 10000;
                    }
                    Thread.Sleep(threadSleepFreq);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }
        public void ThreadCreating()
        {
            Thread mThread = new Thread(DoOperation);
            mThread.Name = "Thread_" + Id.ToString();
            mThread.Start();
        }
    }
}