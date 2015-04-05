using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExcelNamespace = Microsoft.Office.Interop.Excel;

namespace ExportModule.Sevices
{
    public static class ExcelExtensions
    {
        //If excel is pain, then this might help
        //http://forums.asp.net/t/1945979.aspx?Issue+regarding+line+chart+generation+from+datatable+to+excel+c+
        public static ExcelNamespace.Range getRange(this ExcelNamespace.Worksheet ws, int nRows, int nColumns)
        {
            string upperLeftCell = "B2";
            int endRowNumber = System.Int32.Parse(upperLeftCell.Substring(1))
                + nRows - 1;
            char endColumnLetter = System.Convert.ToChar(
                Convert.ToInt32(upperLeftCell[0]) + nColumns - 1);
            string upperRightCell = System.String.Format("{0}{1}",
                endColumnLetter, System.Int32.Parse(upperLeftCell.Substring(1)));
            string lowerRightCell = System.String.Format("{0}{1}",
                endColumnLetter, endRowNumber);
            return ws.get_Range(upperLeftCell, lowerRightCell);
        }


        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
    }
}
