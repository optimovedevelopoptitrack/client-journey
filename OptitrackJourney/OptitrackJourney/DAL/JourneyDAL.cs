using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OptitrackJourney.DAL
{
    public class JourneyDAL : BaseDAL
    {
        internal static class StoredProcedures 
        {
            public const string USP_Optitrack_GetVisitorJourney = "USP_Optitrack_GetVisitorJourneyV2";
        }

        public JourneyDAL(string connectionString) : base(connectionString) { }

        public DataTable GetVisitorJourney(string visitorId, int tenantId)
        {
            var ds = ExecuteMySQLDataTable(StoredProcedures.USP_Optitrack_GetVisitorJourney,true,
                      new MySqlParameter() { ParameterName = "@visitorId", DbType = DbType.String, Value = visitorId },
                      new MySqlParameter() { ParameterName = "@tenantId", DbType = DbType.Int32, Value = tenantId });
            return ds;
        }

    }
}