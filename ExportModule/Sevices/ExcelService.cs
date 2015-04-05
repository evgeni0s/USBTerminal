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
using System.Reflection;
using ExportModule.Sevices;
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

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


        //creating excel file is more complicated then File.Create(path); it needs template
        public bool CreateExcelFile(string path)
        {
            ExcelNamespace.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null || Type.GetTypeFromProgID("Excel.Application") == null)
            {
                _logger.Log("Excel is not properly installed!", Category.Exception, Priority.Medium);
                return false;
            }
            bool result = true;

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                //file is locked. Solution needs time, which do not have right now
            }

            ExcelNamespace.Workbook xlWorkBook;
            //ExcelNamespace.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (ExcelNamespace.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //xlWorkSheet.Cells[1, 1] = "Sheet 1 content";
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
            //releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            _logger.Log("Excel file created , you can find the file: " + path, Category.Debug, Priority.Medium);
            return result;
        }

        #region works perfect backup
        //public void ExportToExcelFile(string path, List<ChartExportArguments> args)
        //{
        //    object misValue = System.Reflection.Missing.Value;
        //    ExcelNamespace.Application excelApp = new ExcelNamespace.Application();
        //    ExcelNamespace.Workbook excelWorkBook = excelApp.Workbooks.Open(path);
        //    /*algo: write cells to worksheet
        //     * 
        //     * 
        //     * get them
        //     chartRange = xlWorkSheet.get_Range("B137:Y137, B139:Y139, B141:Y141", Missing.Value);  I need only 2 columns
        //     * 
        //     * 

        //     */
        //    DataSet ds = new DataSet("Organization");
        //        //Works perfect, but I do not have acces to other props
        //    foreach (var item in args)
        //    {
        //        ds.Tables.Add(item.Points.ToDataTable());
        //    }
        //    //DO NOT DELETE this comment. I can use Name of DataTable and name on rows in Data and even name of properties to reflect them in table
        //    //I started with List<Point> and ended up with excel that had 2 rows with titles "X" and "Y", because class point has props int X and int Y
        //    foreach (DataTable table in ds.Tables)
        //    {
        //        //Add a new worksheet to workbook with the Datatable name
        //        ExcelNamespace.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
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
        //        ExcelNamespace.ChartObjects xlCharts = (ExcelNamespace.ChartObjects)excelWorkSheet.ChartObjects(Type.Missing);
        //        ExcelNamespace.ChartObject myChart = (ExcelNamespace.ChartObject)xlCharts.Add(40, 20, 300, 300);
        //        ExcelNamespace.Chart xlChart = myChart.Chart;
        //        ExcelNamespace.Range chartRange = excelWorkSheet.getRange(table.Rows.Count - 1, 2);
        //        xlChart.SetSourceData(chartRange, Type.Missing);
        //        xlChart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;
        //        releaseObject(excelWorkSheet);
        //    }



        //    //foreach (var item in args)
        //    //{
        //    //ExcelNamespace.Worksheet xlWorkSheet = excelWorkBook.Sheets.Add();
        //    //xlWorkSheet.Name = item.ChartId;
        //    //ExcelNamespace.Range chartRange;

        //    //ExcelNamespace.ChartObjects xlCharts = (ExcelNamespace.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
        //    //ExcelNamespace.ChartObject myChart = (ExcelNamespace.ChartObject)xlCharts.Add(10, 80, 300, 250);
        //    //ExcelNamespace.Chart chartPage = myChart.Chart;

        //    //xlWorkSheet.Cells[1, 1] = item.XName;
        //    //xlWorkSheet.Cells[1, 2] = item.YName;

        //    //int rangeHeight = item.Points.Count();



        //    //chartRange = xlWorkSheet.get_Range("A2", "B" + (rangeHeight + 1).ToString());
        //    //chartPage.SetSourceData(chartRange, misValue);
        //    //chartPage.ChartType = ExcelNamespace.XlChartType.xlColumnClustered;




        //    //excelWorkBook.Save();
        //    excelWorkBook.Close(true, misValue, misValue);
        //    excelApp.Quit();
        //    releaseObject(excelWorkBook);
        //    releaseObject(excelApp);


        //    //}


        //}
        #endregion

        public void ExportToExcelFile(string path, List<ChartExportArguments> args)
        {
            object misValue = System.Reflection.Missing.Value;
            ExcelNamespace.Application excelApp = new ExcelNamespace.Application();
            ExcelNamespace.Workbook excelWorkBook = excelApp.Workbooks.Open(path);
            /*algo: write cells to worksheet
             * 
             * 
             * get them
             chartRange = xlWorkSheet.get_Range("B137:Y137, B139:Y139, B141:Y141", Missing.Value);  I need only 2 columns
             * 
             * 

             */
            //DO NOT DELETE this comment. I can use Name of DataTable and name on rows in Data and even name of properties to reflect them in table
            //I started with List<Point> and ended up with excel that had 2 rows with titles "X" and "Y", because class point has props int X and int Y


            foreach (var item in args)
            {
                DataTable table = item.Points.ToDataTable();
                //Add a new worksheet to workbook with the Datatable name
                ExcelNamespace.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = item.ChartTitle;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
                ExcelNamespace.ChartObjects xlCharts = (ExcelNamespace.ChartObjects)excelWorkSheet.ChartObjects(Type.Missing);
                ExcelNamespace.ChartObject myChart = (ExcelNamespace.ChartObject)xlCharts.Add(item.XOffset_Chart, item.YOffset_Chart, item.Width, item.Height);
                ExcelNamespace.Chart xlChart = myChart.Chart;
                // *******************Set Range: ***********************
                ExcelNamespace.Range chartRange = excelWorkSheet.getRange(table.Rows.Count - 1, 2);
                xlChart.SetSourceData(chartRange, Type.Missing);
                xlChart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

                // *******************Customize axes: ***********************
                ExcelNamespace.Axis xAxis = (ExcelNamespace.Axis)xlChart.Axes(ExcelNamespace.XlAxisType.xlCategory,
                     ExcelNamespace.XlAxisGroup.xlPrimary);
                xAxis.HasTitle = item.XAxis_HasTitle;
                xAxis.AxisTitle.Text = item.XAxis_AxisTitle;

                //error ... this is actual Z. 
                //ExcelNamespace.Axis yAxis = (ExcelNamespace.Axis)xlChart.Axes(ExcelNamespace.XlAxisType.xlSeriesAxis,
                // ExcelNamespace.XlAxisGroup.xlPrimary);
                //yAxis.HasTitle = item.yAxis_HasTitle;
                //yAxis.AxisTitle.Text = item.yAxis_AxisTitle;


                //this works as normal Y axis
                ExcelNamespace.Axis zAxis = (ExcelNamespace.Axis)xlChart.Axes(ExcelNamespace.XlAxisType.xlValue,
                     ExcelNamespace.XlAxisGroup.xlPrimary);
                zAxis.HasTitle = item.YAxis_HasTitle;
                zAxis.AxisTitle.Text = item.YAxis_AxisTitle;

                // *********************Add title: *******************************
                xlChart.HasTitle = item.HasTitle;
                if (item.HasTitle)
                {
                    xlChart.ChartTitle.Text = item.ChartTitle;
                }

                // *****************Set legend:***************************
                xlChart.HasLegend = item.HasLegend;

                releaseObject(excelWorkSheet);
            }
            excelWorkBook.Close(true, misValue, misValue);
            excelApp.Quit();
            releaseObject(excelWorkBook);
            releaseObject(excelApp);

        }

        //private void writeRangeVerticaly(ExcelNamespace.Worksheet xlWorkSheet, List<Point> points, int verticalOffSet)
        //{
        //    for (int i = verticalOffSet; i < points.Count; i++)
        //    {
        //         xlWorkSheet.Cells[i,
        //    }
        //}

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
