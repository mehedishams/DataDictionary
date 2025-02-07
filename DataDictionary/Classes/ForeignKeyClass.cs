﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataDictionary.Classes
{
    class ForeignKeyClass
    {
        public List<FKKeyCriteria> FKList = new List<FKKeyCriteria>();
        string TableName;
        SqlConnection Conn;

        public ForeignKeyClass(string TableNameParam, SqlConnection ConnParam)
        {
            Conn = ConnParam;
            TableName = TableNameParam;
        }

        public void GetForeignKeysAndDependencies()
        {
            SqlCommand Cmd = new SqlCommand();

            //Code adapted from http://stackoverflow.com/questions/483193/how-can-i-list-all-foreign-keys-referencing-a-given-table-in-sql-server
            Cmd.CommandText = "SELECT PKTABLE_QUALIFIER = CONVERT(SYSNAME, DB_NAME()), " +
                              "PKTABLE_OWNER = CONVERT(SYSNAME, SCHEMA_NAME(O1.SCHEMA_ID)), " +
                              "PKTABLE_NAME = CONVERT(SYSNAME, O1.NAME), " +
                              "PKCOLUMN_NAME = CONVERT(SYSNAME, C1.NAME), " +
                              "FKTABLE_QUALIFIER = CONVERT(SYSNAME, DB_NAME()), " +
                              "FKTABLE_OWNER = CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID)), " +
                              "FKTABLE_NAME = CONVERT(SYSNAME, O2.NAME), " +
                              "FKCOLUMN_NAME = CONVERT(SYSNAME, C2.NAME), " +
                              "UPDATE_RULE = CONVERT(SMALLINT, CASE OBJECTPROPERTY(F.OBJECT_ID, 'CnstIsUpdateCascade') " +
                                        "WHEN 1 THEN 0 " +
                                        "ELSE 1 " +
                                        "END), " +
                              "DELETE_RULE = CONVERT(SMALLINT, CASE OBJECTPROPERTY(F.OBJECT_ID, 'CnstIsDeleteCascade') " +
                                        "WHEN 1 THEN 0 " +
                                        "ELSE 1 " +
                                        "END), " +
                              "FK_NAME = CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)), " +
                              "PK_NAME = CONVERT(SYSNAME, I.NAME), " +
                              "DEFERRABILITY = CONVERT(SMALLINT, 7)" +
                              "FROM   SYS.ALL_OBJECTS O1, " +
                              "SYS.ALL_OBJECTS O2, " +
                              "SYS.ALL_COLUMNS C1, " +
                              "SYS.ALL_COLUMNS C2, " +
                              "SYS.FOREIGN_KEYS F " +
                              "INNER JOIN SYS.FOREIGN_KEY_COLUMNS K " +
                              "ON(K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID) " +
                              "INNER JOIN SYS.INDEXES I " +
                              "ON(F.REFERENCED_OBJECT_ID = I.OBJECT_ID " +
                              "AND F.KEY_INDEX_ID = I.INDEX_ID) " +
                              "WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID " +
                              "AND O2.OBJECT_ID = F.PARENT_OBJECT_ID " +
                              "AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID " +
                              "AND C2.OBJECT_ID = F.PARENT_OBJECT_ID " +
                              "AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID " +
                              "AND C2.COLUMN_ID = K.PARENT_COLUMN_ID " +
                              "AND CONVERT(SYSNAME, O2.NAME) = '" + TableName + "'";

            Cmd.Connection = Conn;
            SqlDataAdapter DAdapter = new SqlDataAdapter();
            DataTable DTable = new DataTable();
            DAdapter.SelectCommand = Cmd;
            DAdapter.Fill(DTable);

            foreach (DataRow DR in DTable.Rows)
            {
                FKKeyCriteria KeyItem = new FKKeyCriteria();
                KeyItem.ForeignKeyName = DR["FKCOLUMN_NAME"].ToString();
                KeyItem.PrimaryKeyTable = DR["PKTABLE_NAME"].ToString();
                KeyItem.NameInPrimaryKeyTable = DR["PKCOLUMN_NAME"].ToString();
                FKList.Add(KeyItem);
            }
        }
    }

    class FKKeyCriteria
    {
        public string ForeignKeyName { get; set; }  // The foreign key column name of a table.
        public string PrimaryKeyTable { get; set; }     // The table where this foreign key is used as a primary key.
        public string NameInPrimaryKeyTable { get; set; }   // The name that the table uses this column as a primary key.
    }
}
