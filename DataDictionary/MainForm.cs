///////////////////////////////////////////////////
///////////////////////////////////////////////////
/////////// Coded by: Mehedi Shams Rony ///////////
/////////////////// Bangladesh ////////////////////
/////////////////// Oct, 2016 /////////////////////
///////////////////////////////////////////////////
///////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using DataDictionary.Classes;

namespace DataDictionary
{
    public partial class MainForm : Form
    {
        SqlConnection Conn;
        List<DataDetailsKeyCriteriaClass> ColumnList;
        List<PKKeyCriteria> PKList;
        List<FKKeyCriteria> FKList;
        Responsive ResponsiveObj;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                ResponsiveObj = new Responsive(Screen.PrimaryScreen.Bounds);
                ResponsiveObj.SetMultiplicationFactor();

                TotalSizeLbl.Text = "";
                Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
                Conn.Open();

                LoadDatabases();
                ListTables();
            }
            catch (SqlException Ex)
            {
                MessageBox.Show("An error occurred in 'MainForm' constructor while connecting to database. Error msg: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An error occurred in 'MainForm' constructor. Error msg: " + Ex.Message);
            }
        }

        private void LoadDatabases()
        {
            List<string> DBNames = new List<string>();
            using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", Conn))
            using (IDataReader DR = cmd.ExecuteReader())
            {
                while (DR.Read())
                    DBNames.Add(DR[0].ToString());
            }
            DatabasesCmb.DataSource = DBNames.OrderBy(P => P.ToString()).ToList();  // Sort the items alphabetically and display.
        }

        private void ListTables()
        {
            try
            {
                SqlConnectionStringBuilder SBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
                SBuilder.InitialCatalog = DatabasesCmb.Text;
                Conn = new SqlConnection(SBuilder.ToString());
                Conn.Open();
                
                DataTable schema = Conn.GetSchema(SqlClientMetaDataCollectionNames.Tables);
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                    if (row[3].Equals("BASE TABLE"))    // Only add tables, not views.
                        TableNames.Add(row[2].ToString());
                TablesCmb.DataSource = TableNames.OrderBy(P => P.ToString()).ToList();  // Sort the items alphabetically and display.
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An error occurred in 'ListTables' method. Error msg: " + Ex.Message);
            }
        }

        private void CreateDictionaryBtn_Click(object sender, EventArgs e)
        {
            if (TablesCmb.Text.Substring(0, 1).Equals("#"))
            {
                MessageBox.Show("This is a temporary table; no information available.", ConfigurationManager.AppSettings["APP_TITLE"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                PKGridView.DataSource = FKGridView.DataSource = ColumnsGridView.DataSource = null;
                return;
            }

            try
            {   
                string TotalSize = "";

                // Specify the restrictions.
                string[] restrictions = new string[4];
                restrictions[2] = TablesCmb.Text;                

                DataTable UniqueKeys = Conn.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, restrictions);

                List<string> UniqueIndexList = new List<string>();
                foreach (DataRow UniqueKey in UniqueKeys.Rows)
                    UniqueIndexList.Add(UniqueKey["column_name"].ToString());

                PrimaryKeyClass PrimaryKeyObj = new PrimaryKeyClass(TablesCmb.Text, Conn);
                PrimaryKeyObj.GetPrimaryKeysAndDependencies();
                PKList = PrimaryKeyObj.PKList;
                PKGridView.DataSource = PKList;

                ForeignKeyClass ForeignKeyObj = new ForeignKeyClass(TablesCmb.Text, Conn);
                ForeignKeyObj.GetForeignKeysAndDependencies();
                FKList = ForeignKeyObj.FKList;
                FKGridView.DataSource = FKList;

                DataDetailsClass DataDetailsObj = new DataDetailsClass(DatabasesCmb.Text, TablesCmb.Text, Conn);
                if (ViewHardCodedExample.Checked)
                    DataDetailsObj.GetColumnDetails(PKList, FKList, UniqueIndexList, out TotalSize, DataDetailsClass.EXAMPLE_CHOICE.HardCoded);
                else if (ViewLiveExample.Checked)
                    DataDetailsObj.GetColumnDetails(PKList, FKList, UniqueIndexList, out TotalSize, DataDetailsClass.EXAMPLE_CHOICE.Live);
                ColumnList = DataDetailsObj.ColumnList;
                ColumnsGridView.DataSource = ColumnList;

                foreach (DataGridViewColumn Col in ColumnsGridView.Columns)     // Only the three columns are editable, others are read-only.
                    Col.ReadOnly = !(Col.Name.Equals("Description") || Col.Name.Equals("Example") || Col.Name.Equals("Notes"));

                TotalSizeLbl.Text = TotalSize;
                
                TotalSizeLbl.Refresh();
                Application.DoEvents();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An error occurred in 'CreateDictionaryBtn_Click' method while building the dictionary. Error msg: " + Ex.Message,
                                ConfigurationManager.AppSettings["APP_TITLE"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        
        
        private void ExportToExcelBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog FDialog = new SaveFileDialog();
            FDialog.Filter = "Excel Workbook|*.xlsx|Excel (97-2003)|*.xls";
            FDialog.Title = "Save the data dictionary";

            FDialog.ShowDialog();

            if (FDialog.FileName == "")
            {
                MessageBox.Show("Dictionary was not saved.", ConfigurationManager.AppSettings["APP_TITLE"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ExcelOutputClass ExcelOutputObj = new ExcelOutputClass();
            string ErrMsg = "";
            bool SaveSuccess = ExcelOutputObj.OutputToExcel(FDialog.FileName, TablesCmb.Text, ColumnsGridView, PKGridView, FKGridView, TotalSizeLbl.Text, out ErrMsg);
            if (SaveSuccess)
            {
                MessageBox.Show("Dictionary was saved successfully!", ConfigurationManager.AppSettings["APP_TITLE"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("An error occurred in 'ExportToExcelBtn_Click' method whileexporting the data dictionary to file. Error msg: " + ErrMsg);
            }
        }

        private void TablesCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateDictionaryBtn_Click(sender, e);
        }

        private void DatabasesCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListTables();
        }

        private void ColumnsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Code adapted from http://stackoverflow.com/questions/16105718/datagridview-changing-cell-background-color
            if (e.Value != null && e.Value.ToString().Equals("Yes"))
                e.CellStyle.BackColor = Color.Green;
        }

        private void ColumnsGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Put each of the columns into programmatic sort mode.
            foreach (DataGridViewColumn column in ColumnsGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        #region Grid sort events
        // http://stackoverflow.com/questions/8011481/how-to-sort-a-column-in-datagridview-that-is-bound-to-a-list-in-winform
        private void ColumnsGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SorGrid(ColumnsGridView.Columns[e.ColumnIndex], ColumnsGridView);
        }

        private void PKGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SorGrid(PKGridView.Columns[e.ColumnIndex], PKGridView);
        }

        private void FKGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SorGrid(FKGridView.Columns[e.ColumnIndex], FKGridView);
        }

        private void SorGrid(DataGridViewColumn Col, DataGridView GridView)
        {
            System.Windows.Forms.SortOrder sortOrder = Col.HeaderCell.SortGlyphDirection == System.Windows.Forms.SortOrder.Ascending
                                      ? System.Windows.Forms.SortOrder.Descending
                                      : System.Windows.Forms.SortOrder.Ascending;

            switch (GridView.Name)
            {
                case "ColumnsGridView":
                    ColumnList.Sort(new SortableDataDetailsListClass(Col.Name, sortOrder)); break;
                case "PKGridView":
                    PKList.Sort(new SortablePKListClass(Col.Name, sortOrder)); break;
                case "FKGridView":
                    FKList.Sort(new SortableFKListClass(Col.Name, sortOrder)); break;
            }

            GridView.Refresh();
            Col.HeaderCell.SortGlyphDirection = sortOrder;  // Display up arrow or down arrow.
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            Width = ResponsiveObj.GetMetrics(Width, "Width");    // Form width and height set up.
            Height = ResponsiveObj.GetMetrics(Height, "Height");
            Left = Screen.GetBounds(this).Width / 2 - Width / 2;  // Form centering.
            Top = Screen.GetBounds(this).Height / 2 - Height / 2 - 30;  // 30 is a calibration factor.

            foreach (Control Ctl in this.Controls)
            {
                Ctl.Font = new Font(FontFamily.GenericSansSerif, ResponsiveObj.GetMetrics((int)Ctl.Font.Size), FontStyle.Regular);
                Ctl.Width = ResponsiveObj.GetMetrics(Ctl.Width, "Width");
                Ctl.Height = ResponsiveObj.GetMetrics(Ctl.Height, "Height");
                Ctl.Top = ResponsiveObj.GetMetrics(Ctl.Top, "Top");
                Ctl.Left = ResponsiveObj.GetMetrics(Ctl.Left, "Left");
            }
        }

        private void ViewHardCodedExample_CheckedChanged(object sender, EventArgs e)
        {
            CreateDictionaryBtn_Click(sender, e);
        }

        private void ViewLiveExample_CheckedChanged(object sender, EventArgs e)
        {
            CreateDictionaryBtn_Click(sender, e);
        }
    }
}