///////////////////////////////////////////////////
///////////////////////////////////////////////////
/////////// Coded by: Mehedi Shams Rony ///////////
/////////////////// Bangladesh ////////////////////
/////////////////// Oct, 2016 /////////////////////
///////////////////////////////////////////////////
///////////////////////////////////////////////////
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataDictionary.Classes
{
    class PrimaryKeyClass
    {
        public List<PKKeyCriteria> PKList = new List<PKKeyCriteria>();
        string TableName;
        SqlConnection Conn;

        public PrimaryKeyClass(string TableNameParam, SqlConnection ConnParam)
        {
            Conn = ConnParam;
            TableName = TableNameParam;
        }

        public void GetPrimaryKeysAndDependencies()
        {
            SqlDataAdapter DAdapter = new SqlDataAdapter();
            DataTable DTable = new DataTable();
            using (var Cmd = new SqlCommand("sp_fkeys", Conn))
            {
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@pktable_name", TableName);
                DAdapter.SelectCommand = Cmd;
                DAdapter.Fill(DTable);
            }

            foreach (DataRow DR in DTable.Rows)
            {
                PKKeyCriteria KeyItem = new PKKeyCriteria();
                KeyItem.PrimaryKeyName = DR["PKCOLUMN_NAME"].ToString();
                KeyItem.ForeignKeyTable = DR["FKTABLE_NAME"].ToString();
                KeyItem.NameInForeignKeyTable = DR["FKCOLUMN_NAME"].ToString();
                PKList.Add(KeyItem);
            }

            // Soemtimes dependencies might not be obtained for primary keys from the above method. Following is a workaround. However, we don't have
            // any information about the foreign table and column name in such cases.
            if (PKList == null || PKList.Count == 0)
                using (var cmd = new SqlCommand("SELECT * FROM [" + TableName + "]", Conn))
                {
                    DAdapter = new SqlDataAdapter();
                    DAdapter.SelectCommand = cmd;
                    DAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;    // Optional, but as a safety measure.

                    var dtab = new DataTable();
                    DAdapter.FillSchema(dtab, SchemaType.Source); // Only the schema.

                    foreach (DataColumn col in dtab.PrimaryKey)
                    {
                        PKKeyCriteria KeyItem = new PKKeyCriteria();
                        KeyItem.PrimaryKeyName = col.ColumnName;
                        PKList.Add(KeyItem);
                    }
                }
        }
    }

    class PKKeyCriteria
    {
        public string PrimaryKeyName { get; set; }  // The primary key column name of a table.
        public string ForeignKeyTable { get; set; }     // The table where this primary key is used as a foreign key.
        public string NameInForeignKeyTable { get; set; }   // The column name that the foreign table uses for this primary key.
    }
}
