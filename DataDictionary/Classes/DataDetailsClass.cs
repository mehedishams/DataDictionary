///////////////////////////////////////////////////
///////////////////////////////////////////////////
/////////// Coded by: Mehedi Shams Rony ///////////
/////////////////// Bangladesh ////////////////////
/////////////////// Oct, 2016 /////////////////////
///////////////////////////////////////////////////
///////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataDictionary.Classes
{
    class DataDetailsClass
    {
        public List<DataDetailsKeyCriteriaClass> ColumnList = new List<DataDetailsKeyCriteriaClass>();
        string DBName;
        string TableName;
        SqlConnection Conn;
        public enum EXAMPLE_CHOICE {Live, HardCoded};

        public DataDetailsClass(string DBNameParam, string TableNameParam, SqlConnection ConnParam)
        {
            Conn = ConnParam;
            DBName = DBNameParam;
            TableName = TableNameParam;
        }

        public void GetColumnDetails(List<PKKeyCriteria> PKList, List<FKKeyCriteria> FKList, List<string> UniqueIndexList, out string DiskSpace, EXAMPLE_CHOICE ExampleChoiceParam)
        {
            DiskSpace = "";
            List<string> ComputedColumns = new List<string>();
            GetComputedColumns(ref ComputedColumns);
            using (var cmd = new SqlCommand("SELECT * FROM [" + TableName + "]", Conn))
            using (var DReader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                DataTable table = DReader.GetSchemaTable();   // Get the column meta-data only.

                int Index = 0;
                foreach (DataRow row in table.Rows)
                {
                    DataDetailsKeyCriteriaClass Details = new DataDetailsKeyCriteriaClass();
                    Details.ColumnName = row["ColumnName"].ToString();

                    if (ComputedColumns.FindIndex(p => p == Details.ColumnName) == -1) // If the current column is not a computed column.
                    {
                        Details.ColumnSize = Convert.ToInt64(row["ColumnSize"]);
                        Details.DataType = ((Type)row["DataType"]).Name;
                        Details.SqlDataType = row["DataTypeName"].ToString();

                        Details.Description = GetKeyValueFromAppConfig(Details.SqlDataType + "_Description", Details.ColumnSize);
                        Details.Example = ExampleChoiceParam == EXAMPLE_CHOICE.HardCoded ? GetKeyValueFromAppConfig(Details.SqlDataType + "_Example") : Details.Example = GetLiveExample(Details.ColumnName);

                        Details.RangeFrom = GetKeyValueFromAppConfig(Details.SqlDataType + "_RangeFrom");
                        Details.RangeTo = GetKeyValueFromAppConfig(Details.SqlDataType + "_RangeTo");

                        Details.Mandatory = row["AllowDBNull"].ToString().Equals("True") ? "No" : "Yes";

                        Index = PKList.FindIndex(Item => Item.PrimaryKeyName.Equals(Details.ColumnName));
                        Details.PrimaryKey = (Index != -1) ? "Yes" : "No";

                        Index = UniqueIndexList.FindIndex(Item => Item.Equals(Details.ColumnName));
                        Details.UniqueKey = (Index != -1) ? "Yes" : "No";

                        Index = FKList.FindIndex(Item => Item.ForeignKeyName.Equals(Details.ColumnName));
                        Details.ForeignKey = (Index != -1) ? "Yes" : "No";

                        Details.Computed = "No";
                    }
                    else
                    {
                        Details.Computed = "Yes";
                        if (ThisIsAPersistedComputedColumn(Details.ColumnName)) // If only the computed column is a persisted one, then it would occupy disk space.
                            Details.ColumnSize = Convert.ToInt64(row["ColumnSize"]);
                    }
                    ColumnList.Add(Details);
                }
            }

            DiskSpace = RetrieveDiskSpaceDetails();
        }

        private bool ThisIsAPersistedComputedColumn(string ColumnName)
        {
            using (SqlConnection Conn2 = new SqlConnection(Conn.ConnectionString))
            {
                Conn2.Open();
                // http://stackoverflow.com/questions/5272776/how-to-check-if-computed-column-is-persisted
                using (SqlCommand cmd = new SqlCommand("SELECT t.name as TableName, c.name as ColumnName FROM sys.tables t INNER JOIN sys.computed_columns c on c.object_id = t.object_id " +
                                                       "WHERE t.name = '" + TableName + "' AND c.is_persisted = 1", Conn2))
                using (SqlDataReader DReader = cmd.ExecuteReader())
                {
                    while (DReader.Read())
                        if (DReader["ColumnName"].ToString().Equals(ColumnName)) return true;   // The computed column is listed as a persisted column.
                }
            }
            return false;
        }

        private string RetrieveDiskSpaceDetails()
        {
            StringBuilder SBuilder = new StringBuilder();
            double NumRows = 0;
            // http://www.sqlserver-dba.com/2013/07/calculate-sql-row-size.html
            using (SqlCommand cmd = new SqlCommand("select * from sys.dm_db_index_physical_stats(DB_ID(N'" + DBName + "'), OBJECT_ID(N'" + TableName + "'), NULL, NULL , 'DETAILED');", Conn))
            //using (SqlCommand cmd = new SqlCommand("dbcc showcontig ('" + TableName + "') with tableresults", Conn))
            using (SqlDataReader DReader = cmd.ExecuteReader())
                while (DReader.Read())
                {
                    NumRows = Convert.ToInt32(DReader["record_count"]);
                    long TotalSpace = (long)(NumRows * Convert.ToDouble(DReader["avg_record_size_in_bytes"]));
                    SBuilder.Append("Stats (acc. physical state) from [sys.dm_db_index_physical_stats]:\n============================================================\n");
                    SBuilder.Append("Total records: " + NumRows + "." +
                        "\nAvg record size: " + DReader["avg_record_size_in_bytes"] + " bytes." +
                        "\nMin record size: " + DReader["min_record_size_in_bytes"] + " bytes." +
                        "\nMax record size: " + DReader["max_record_size_in_bytes"] + " bytes." +
                        "\nTotal Disk space occupied: " + TotalSpace + " bytes = " +
                        Math.Round((double)TotalSpace / 1024, 2) + " KB = " +
                        Math.Round((double)TotalSpace / (1024 * 1024), 4) + " MB.\n\n\n");
                    break;
                }

            //https://msdn.microsoft.com/en-us/library/ms188776.aspx?f=255&MSPPError=-2147217396
            using (SqlCommand cmd = new SqlCommand("EXEC sp_spaceused N'" + TableName + "'", Conn))
            using (SqlDataReader DReader = cmd.ExecuteReader())
                while (DReader.Read())
                {
                    double SpaceReserved = Convert.ToInt32(DReader["reserved"].ToString().Substring(0, DReader["reserved"].ToString().Length - 3));  // In KB.
                    double DataSpace = Convert.ToInt32(DReader["data"].ToString().Substring(0, DReader["data"].ToString().Length - 3)) * 1024;  // In bytes; data is in KB.
                    double IndexSpace = Convert.ToInt32(DReader["index_size"].ToString().Substring(0, DReader["index_size"].ToString().Length - 3)) * 1024;  // In bytes; data is in KB.

                    SBuilder.Append("Stats (acc. reservation) from [sp_spaceused] SP:\n============================================================\n");

                    if (NumRows == 0)
                        SBuilder.Append("Total allocated space (including index): " + SpaceReserved + " bytes." +
                            "\nAvg record size (including index): 0 bytes." +
                            "\nAvg record size (excluding index): 0 bytes.");
                    else
                        SBuilder.Append("Total allocated space (inc. index): " + SpaceReserved + " KB." +
                            "\nAvg record size (inc. index): " + Math.Round((DataSpace + IndexSpace) / NumRows, 2) + " bytes." +
                            "\nAvg record size (ex. index): " + Math.Round(DataSpace / NumRows, 2) + " bytes.");
                }

            return SBuilder.ToString();
        }

        private string GetLiveExample(string ColumnName)
        {
            using (SqlConnection Conn2 = new SqlConnection(Conn.ConnectionString))
            {
                Conn2.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 " + ColumnName + " FROM [" + TableName + "] WHERE " + ColumnName + " IS NOT NULL", Conn2))
                using (SqlDataReader DReader = cmd.ExecuteReader())
                    while (DReader.Read())  // Shortening examples - string examples can be quite large.
                        if (DReader[ColumnName].ToString().Length > 50)
                            return DReader[ColumnName].ToString().Substring(0, 50);
                        else
                            return DReader[ColumnName].ToString();
            }
            return "";
        }

        private void GetComputedColumns(ref List<string> ComputedColumns)
        {
            // http://stackoverflow.com/questions/1484147/get-list-of-computed-columns-in-database-table-sql-server
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM sys.columns WHERE is_computed = 1 AND object_id = OBJECT_ID('" + TableName + "')", Conn))
            using (SqlDataReader DReader = cmd.ExecuteReader())
            {
                while (DReader.Read())
                    ComputedColumns.Add(DReader["name"].ToString());
            }
        }

        private string GetKeyValueFromAppConfig(string Key, long ColumnSize)
        {
            // If the data type is not found, then it returns null, which ultimately displays blank.
            string Description = ConfigurationManager.AppSettings[Key];

            // If a description is found, and it is a description for string types (determined by 'Max' at the beginning), then insert the size.
            if (!string.IsNullOrEmpty(Description) && Description.Substring(0, 3).Equals("Max")) Description = Description.Insert(4, ColumnSize.ToString() + " ");
            return Description;
        }

        private string GetKeyValueFromAppConfig(string Key)
        {
            return ConfigurationManager.AppSettings[Key]; // If the data type is not found, then it returns null, which ultimately displays blank.
        }
    }
    
    class DataDetailsKeyCriteriaClass
    {
        public string ColumnName { get; set; }
        public long ColumnSize { get; set; }
        public string DataType { get; set; }
        public string SqlDataType { get; set; }
        public string Mandatory { get; set; }
        public string PrimaryKey { get; set; }
        public string UniqueKey { get; set; }
        public string ForeignKey { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public string RangeFrom { get; set; }
        public string RangeTo { get; set; }
        public string Notes { get; set; }
        public string Computed { get; set; }
    }
}
