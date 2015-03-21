//using Excel.Models;
using Interfaces.Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExcelNamespace = Microsoft.Office.Interop.Excel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
//using Microsoft.Office.Interop.Excel;

namespace ExportModule.Services
{
    //Instructions of how to export
    //http://www.codeproject.com/Reference/753207/Export-DataSet-into-Excel-using-Csharp-Excel-Inter
    //DataSet can have multiple tables
    internal class ExcelService : IExcelService
    {
        #region old


        //IMeasurementsFeedService _measurmentsService;

        //public List<ExcelItem> ExcelItems { get; set; }


        //public void AddItem(string id)
        //{
        //    ExcelItems.Add(new ExcelItem() { ChartId = id });
        //}

        //public void Export()
        //{


        //    //Create an Excel workbook instance and open it from the predefined location


        //    //Example C:\Users\Zhenja\Documents\USBTerminal\Config\usbt2_03-05-1989-10-30-15
        //    //string filePath = Properties.Settings.Default.DataExportFolder + Properties.Settings.Default.ExportFilePrefix + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
        //    //try
        //    //{
        //    //    if (File.Exists(filePath))
        //    //    {
        //    //        File.Create(filePath);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.ToString());
        //    //    return;
        //    //}

        //    //export(filePath);
        //}

        //public void ExportTo()
        //{
        //    //WPF_Dialogs.Dialogs.FolderBrowseDialog f = new WPF_Dialogs.Dialogs.FolderBrowseDialog();
        //    //WPF_Dialogs.EDialogResult result = f.showDialog();
        //    //if (result == WPF_Dialogs.EDialogResult.OK)
        //    //{
        //    //    string filePath = f.SelectedPath + Properties.Settings.Default.ExportFilePrefix + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
        //    //    export(filePath);
        //    //}
        //}

        ////MY best is to copy existing file
        //private void export(string filePath)
        //{
        //    //if (File.Exists(filePath))
        //    //{
        //    //    File.Create(filePath);
        //    //}
        //    //Creae an Excel application instance
        //    ExcelAPI.Application excelApp = new ExcelAPI.Application();
        //    //ExcelAPI.Workbook excelWorkBook = excelApp.Workbooks.Open(filePath);
        //    ExcelAPI.Workbook excelWorkBook = excelApp.Workbooks.
        //    //var workbook = new ExcelFile();

        //    //// Add new worksheet to Excel file.
        //    //var worksheet = workbook.Worksheets.Add("New worksheet");

        //    //// Set the value of the cell "A1".
        //    //worksheet.Cells["A1"].Value = "Hello world!";

        //    //// Save Excel file.
        //    //workbook.Save("Workbook.xls");
        //    foreach (var item in ExcelItems)
        //    {
        //        var table = _measurmentsService.GetData(item.ChartId);
        //        //Add a new worksheet to workbook with the Datatable name
        //        ExcelAPI.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
        //        excelWorkSheet.Name = table.TableName;

        //        for (int i = 1; i < table.Columns.Count + 1; i++)
        //        {
        //            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
        //        }

        //        for (int j = 0; j < table.Rows.Count; j++)
        //        {
        //            for (int k = 0; k < table.Columns.Count; k++)
        //            {
        //                excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
        //            }
        //        }
        //    }

        //    excelWorkBook.Save();
        //    excelWorkBook.Close();
        //    excelApp.Quit();

        //}
        #endregion
        #region Backup last week
        //private ILoggerFacade _logger;
        //public ExcelService(ILoggerFacade logger)
        //{
        //   _logger = logger;
        //}


        //public bool CreateDocument(List<LineGraph> selectedGraphs, string path)
        //{
        //    bool result = false;
        //    try
        //    {
        //        //if (!File.Exists(path))
        //        //{
        //        //    File.Create(path);
        //        //}
        //        //Creae an Excel application instance
        //        ExcelAPI.Application excelApp = new ExcelAPI.Application();
        //        ExcelAPI.Workbook excelWorkBook = excelApp.Workbooks.Open(path);
        //        //ExcelAPI.Worksheet ws = excelWorkBook.Sheets.Add();
        //        Random random = new Random();

        //        //Create an Emplyee DataTable
        //        DataTable employeeTable = new DataTable("Employee");
        //        employeeTable.Columns.Add("Employee ID");
        //        employeeTable.Columns.Add("Employee Name");
        //        employeeTable.Rows.Add("1", "ABC");
        //        employeeTable.Rows.Add("2", "DEF");
        //        employeeTable.Rows.Add("3", "PQR");
        //        employeeTable.Rows.Add("4", "XYZ");

        //        //Create a Department Table
        //        DataTable departmentTable = new DataTable("Department");
        //        departmentTable.Columns.Add("Department ID");
        //        departmentTable.Columns.Add("Department Name");
        //        departmentTable.Rows.Add("1", "IT");
        //        departmentTable.Rows.Add("2", "HR");
        //        departmentTable.Rows.Add("3", "Finance");

        //        //Create a DataSet with the existing DataTables
        //        DataSet ds = new DataSet("Organization");
        //        ds.Tables.Add(employeeTable);
        //        ds.Tables.Add(departmentTable);

        //        foreach (DataTable table in ds.Tables)
        //        {
        //            //Add a new worksheet to workbook with the Datatable name
        //            ExcelAPI.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
        //            excelWorkSheet.Name = table.TableName;

        //            for (int i = 1; i < table.Columns.Count + 1; i++)
        //            {
        //                excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
        //            }

        //            for (int j = 0; j < table.Rows.Count; j++)
        //            {
        //                for (int k = 0; k < table.Columns.Count; k++)
        //                {
        //                    excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
        //                }
        //            }
        //        }


        //        excelWorkBook.Save();
        //        excelWorkBook.Close();
        //        excelApp.Quit();
        //        result = true;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Log("Cant generate excel!", Category.Exception, Priority.Medium);
        //        _logger.Log(e.Message, Category.Exception, Priority.Medium);
        //    }
        //    return result;
        //}
        #endregion




        private ILoggerFacade _logger;
        public ExcelService(ILoggerFacade logger)
        {
            _logger = logger;
        }


        public bool CreateExcelFile(string path)
        {
            ExcelNamespace.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null || Type.GetTypeFromProgID("Excel.Application") == null)
            {
                _logger.Log("Excel is not properly installed!", Category.Exception, Priority.Medium);
                return false;
            }
            bool result = true;

            ExcelNamespace.Workbook xlWorkBook;
            ExcelNamespace.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (ExcelNamespace.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "Sheet 1 content";
            try
            {
                xlWorkBook.SaveAs(path, ExcelNamespace.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, ExcelNamespace.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (Exception e)
            {
                _logger.Log("Cant save Excel file!", Category.Exception, Priority.Medium);
                _logger.Log("Message: " + e.Message, Category.Exception, Priority.Medium);
                _logger.Log("InnerMessage: " + e.InnerException, Category.Exception, Priority.Medium);
                result = false;
            }
            finally
            {
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }
            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            _logger.Log("Excel file created , you can find the file: " + path, Category.Debug, Priority.Medium);
            return result;
        }

        public void ExportToExcelFile(string path)
        {
            throw new NotImplementedException();
        }



        #region private methods
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion
    }
}
