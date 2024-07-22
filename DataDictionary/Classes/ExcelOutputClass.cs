///////////////////////////////////////////////////
///////////////////////////////////////////////////
/////////// Code Adapted by: Mehedi Shams Rony ////
/////////////////// Bangladesh ////////////////////
/////////////////// Oct, 2016 /////////////////////
///////////////////////////////////////////////////
///////////////////////////////////////////////////
// Code adapted from http://www.c-sharpcorner.com/uploadfile/hrojasara/export-datagridview-to-excel-in-c-sharp/
// Code adapted from https://msdn.microsoft.com/en-us/library/sfezx97z%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
// Code adapted from http://stackoverflow.com/questions/28765580/export-formatted-datagridview-to-excel
// Code adapted from http://csharp.net-informations.com/excel/csharp-format-excel.htm
// Code adapted from http://stackoverflow.com/questions/16566773/changing-column-widths-of-excel-when-exporting-data-to-excel-file-c
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataDictionary.Classes
{
    class ExcelOutputClass
    {
        public bool OutputToExcel(string FileName, string WorksheetName, DataGridView ColumnsGridView,
            DataGridView PKGridView, DataGridView FKGridView, string TotalSize, out string ErrorMsg)
        {
            try
            {
                ErrorMsg = "";

                // creating Excel Application
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                // creating new WorkBook within Excel application
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                // creating new Excelsheet in workbook
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                // see the excel sheet behind the program
                app.Visible = true;
                app.AskToUpdateLinks = false;
                app.DisplayAlerts = false;

                // get the reference of first sheet. By default its name is Sheet1.
                // store its reference to worksheet
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Columns.AutoFit();

                // changing the name of active sheet
                worksheet.Name = WorksheetName;

                // storing header part in Excel
                for (int i = 1; i < ColumnsGridView.Columns.Count + 1; i++)
                    worksheet.Cells[1, i] = ColumnsGridView.Columns[i - 1].HeaderText;

                // storing Each row and column value to excel sheet
                for (int i = 0; i < ColumnsGridView.Rows.Count; i++)
                    for (int j = 0; j < ColumnsGridView.Columns.Count; j++)
                        if (ColumnsGridView.Rows[i].Cells[j].Value != null)
                        {
                            worksheet.Cells[i + 2, j + 1] = ColumnsGridView.Rows[i].Cells[j].Value.ToString();
                            if (ColumnsGridView.Rows[i].Cells[j].Value.ToString().Equals("Yes"))    // Back colour of "Yes" cells to green.
                                worksheet.Cells[i + 2, j + 1].Interior.Color = ColorTranslator.ToOle(Color.Green);
                        }

                // Now export the primary keys references.
                // storing header part in Excel
                for (int i = 1; i < PKGridView.Columns.Count + 1; i++)
                    worksheet.Cells[ColumnsGridView.Rows.Count + 5, i] = PKGridView.Columns[i - 1].HeaderText;

                // storing Each row and column value to excel sheet
                for (int i = 0; i < PKGridView.Rows.Count; i++)
                    for (int j = 0; j < PKGridView.Columns.Count; j++)
                        if (PKGridView.Rows[i].Cells[j].Value != null)
                            worksheet.Cells[i + 1 + ColumnsGridView.Rows.Count + 5, j + 1] = PKGridView.Rows[i].Cells[j].Value.ToString();

                // Now export the foreign keys references.
                // storing header part in Excel
                for (int i = 1; i < FKGridView.Columns.Count + 1; i++)
                    worksheet.Cells[ColumnsGridView.Rows.Count + 5, i + 7] = FKGridView.Columns[i - 1].HeaderText;

                // storing Each row and column value to excel sheet
                for (int i = 0; i < FKGridView.Rows.Count; i++)
                    for (int j = 0; j < FKGridView.Columns.Count; j++)
                        if (FKGridView.Rows[i].Cells[j].Value != null)
                            worksheet.Cells[i + 1 + ColumnsGridView.Rows.Count + 5, j + 1 + 7] = FKGridView.Rows[i].Cells[j].Value.ToString();

                // formatting the header of columns.
                Microsoft.Office.Interop.Excel.Range formatRange;

                // formatting the header of primary key references.
                worksheet.Cells[(ColumnsGridView.Rows.Count + 4), 1] = "Primary Key References";  // Putting a header
                formatRange = worksheet.get_Range("A" + (ColumnsGridView.Rows.Count + 4) + ":" + "A" + (ColumnsGridView.Rows.Count + 4));
                formatRange.Font.Bold = true;
                formatRange.Interior.Color = ColorTranslator.ToOle(Color.Gray);   // Header backcolour to Gray.
                formatRange.Font.Color = ColorTranslator.ToOle(Color.White);      // Header forecolour to white.

                formatRange = worksheet.get_Range("A1:" + (char)(ColumnsGridView.Columns.Count - 1 + 'A') + "1");  // The range of columns. E.g. A1:M1);
                formatRange.Font.Bold = true;
                formatRange.Interior.Color = ColorTranslator.ToOle(Color.Gray);   // Header backcolour to Gray.
                formatRange.Font.Color = ColorTranslator.ToOle(Color.White);      // Header forecolour to white.

                // formatting the header of foreign key references.
                worksheet.Cells[(ColumnsGridView.Rows.Count + 4), PKGridView.Columns.Count + 5] = "Foreign Key References";  // Putting a header
                formatRange = worksheet.get_Range((char)(PKGridView.Columns.Count + 4 + 'A') + "" + (ColumnsGridView.Rows.Count + 4) + ":" +
                    (char)(PKGridView.Columns.Count + 4 + 'A') + "" + (ColumnsGridView.Rows.Count + 4));
                formatRange.Font.Bold = true;
                formatRange.Interior.Color = ColorTranslator.ToOle(Color.Gray);   // Header backcolour to Gray.
                formatRange.Font.Color = ColorTranslator.ToOle(Color.White);      // Header forecolour to white.

                formatRange = worksheet.get_Range("A" + (ColumnsGridView.Rows.Count + 5) + ":" +
                    (char)(PKGridView.Columns.Count - 1 + 'A') + (ColumnsGridView.Rows.Count + 5));  // The range of columns. E.g. A25:C25
                formatRange.Font.Bold = true;
                formatRange.Interior.Color = ColorTranslator.ToOle(Color.Gray);   // Header backcolour to Gray.
                formatRange.Font.Color = ColorTranslator.ToOle(Color.White);      // Header forecolour to white.

                // formatting the header of foreign key references.
                formatRange = worksheet.get_Range((char)('A' + PKGridView.Columns.Count + 4) + "" + (ColumnsGridView.Rows.Count + 5) + ":" +
                    (char)('A' + PKGridView.Columns.Count + 5 + PKGridView.Columns.Count) + "" + (ColumnsGridView.Rows.Count + 5));  // The range of columns. E.g. H25:J25
                formatRange.Font.Bold = true;
                formatRange.Interior.Color = ColorTranslator.ToOle(Color.Gray);   // Header backcolour to Gray.
                formatRange.Font.Color = ColorTranslator.ToOle(Color.White);      // Header forecolour to white.                

                // Autofitting the cells so that everything is visible.
                formatRange = worksheet.get_Range("A:" + (char)(ColumnsGridView.Columns.Count - 1 + 'A'));  // The range of columns.
                formatRange.Columns.AutoFit();

                // Putting the rough estimate of disk space occupied by a single record of the table.
                worksheet.Cells[(ColumnsGridView.Rows.Count + PKGridView.Rows.Count + 10), 1] = TotalSize;

                // save the application
                workbook.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // Exit from the application
                app.Quit();
                return true;
            }
            catch (Exception Ex)
            {
                ErrorMsg = Ex.Message;
                return false;
            }
            finally
            {
                GC.Collect();   // Free memory.
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
