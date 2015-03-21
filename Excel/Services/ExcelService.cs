using Excel.Models;
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
using ExcelAPI = Microsoft.Office.Interop.Excel;

namespace Excel
{
    //Instructions of how to export
    //http://www.codeproject.com/Reference/753207/Export-DataSet-into-Excel-using-Csharp-Excel-Inter
    //DataSet can have multiple tables
    public class ExcelService : IExcelService
    {
        #region old


        IMeasurementsFeedService _measurmentsService;

        public List<ExcelItem> ExcelItems { get; set; }


        public void AddItem(string id)
        {
            ExcelItems.Add(new ExcelItem() { ChartId = id });
        }

        public void Export()
        {


            //Create an Excel workbook instance and open it from the predefined location


            //Example C:\Users\Zhenja\Documents\USBTerminal\Config\usbt2_03-05-1989-10-30-15
            string filePath = Properties.Settings.Default.DataExportFolder + Properties.Settings.Default.ExportFilePrefix + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
            try
            {
                if (File.Exists(filePath))
                {
                    File.Create(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

            export(filePath);
        }

        public void ExportTo()
        {
            //WPF_Dialogs.Dialogs.FolderBrowseDialog f = new WPF_Dialogs.Dialogs.FolderBrowseDialog();
            //WPF_Dialogs.EDialogResult result = f.showDialog();
            //if (result == WPF_Dialogs.EDialogResult.OK)
            //{
            //    string filePath = f.SelectedPath + Properties.Settings.Default.ExportFilePrefix + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
            //    export(filePath);
            //}
        }

        private void export(string filePath)
        {
            //Creae an Excel application instance
            ExcelAPI.Application excelApp = new ExcelAPI.Application();
            ExcelAPI.Workbook excelWorkBook = excelApp.Workbooks.Open(filePath);

            foreach (var item in ExcelItems)
            {
                DataTable table = _measurmentsService.GetData(item.ChartId);
                //Add a new worksheet to workbook with the Datatable name
                ExcelAPI.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

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
            }

            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApp.Quit();

        }
        #endregion

        private ILoggerFacade _logger;
        public ExcelService(ILoggerFacade logger)
        {
           _logger = logger;
        }


        public bool CreateDocument(List<LineGraph> selectedGraphs, string path)
        {
            bool result = false;
            try
            {
                //Creae an Excel application instance
                ExcelAPI.Application excelApp = new ExcelAPI.Application();
                ExcelAPI.Workbook excelWorkBook = excelApp.Workbooks.Open(path);

                foreach (var item in ExcelItems)
                {
                    DataTable table = _measurmentsService.GetData(item.ChartId);
                    //Add a new worksheet to workbook with the Datatable name
                    ExcelAPI.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                    excelWorkSheet.Name = table.TableName;

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
                }
                excelWorkBook.Save();
                excelWorkBook.Close();
                excelApp.Quit();
                result = true;
            }
            catch (Exception e)
            {
                _logger.Log("Cant generate excel!", Category.Exception, Priority.Medium);
                _logger.Log(e.Message, Category.Exception, Priority.Medium);
            }
            return result;
        }
    }
}
