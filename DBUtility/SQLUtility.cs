using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DBUtility
{
    public class SQLUtility
    {
        public static object IsDBNull(string v)
        {
            if (string.IsNullOrEmpty(v))
                return DBNull.Value;
            else
                return v;
        }

        public static bool ReaderExists(SqlDataReader dr, string columnName)
        {

            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" +

            columnName + "'";

            return (dr.GetSchemaTable().DefaultView.Count > 0);

        }
    }
}
