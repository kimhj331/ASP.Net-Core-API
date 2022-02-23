using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ButlerLee.API.Helpers
{
    public static class ExcelImporter
    {
        public static DataTable Import(Stream inputStream, string sheetName = "", int headerRowIndex = 0)
        {
            XLWorkbook workBook = new XLWorkbook(inputStream);
            workBook.CalculateMode = XLCalculateMode.Auto;

            IXLWorksheet worksheet;
            if (string.IsNullOrEmpty(sheetName) == true)
                worksheet = workBook.Worksheets.FirstOrDefault();
            else
            {
                worksheet =
                    workBook.Worksheets
                        .Where(o => o.Name.ToLower().Equals(sheetName.ToLower()))
                        .SingleOrDefault();

                if (worksheet == null)
                    worksheet = workBook.Worksheets.FirstOrDefault();
            }

            DataTable dataTable = new DataTable();
            int rowIndex = 0;
            try
            {
                if (worksheet.FirstCellUsed() == null || worksheet.LastCellUsed() == null)
                    return null;

                IXLCells cells = worksheet.CellsUsed();
                IXLRange range = worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed());

                int columnCount = range.ColumnCount();
                int rowCount = range.RowCount();

                dataTable.Clear();

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    DataColumn column = new DataColumn();
                    dataTable.Columns.Add(column);
                }

                // add rows data
                foreach (IXLRangeRow item in range.Rows())
                {
                    if (rowIndex == 0 || rowIndex <= headerRowIndex)
                    {
                        rowIndex++;
                        continue;
                    }

                    object[] array = new object[columnCount];
                    for (int y = 0; y < columnCount; y++)
                    {
                        //컬럼명 설정
                        //if (rowIndex == headertRowIndex)
                        //    dataTable.Columns[y].ColumnName = item.Cell(y + 1).Value.ToString();
                        IXLCell cell = item.Cell(y + 1);

                        try
                        {
                            if (rowIndex > headerRowIndex)
                            {
                                if (cell.HasFormula == false)
                                    array[y] = cell.Value;
                                else
                                {
                                    array[y] = cell.CachedValue;
                                }
                            }
                        }
                        catch
                        {
                            array[y] = "";
                        }
                    }

                    if (rowIndex > 0)
                        dataTable.Rows.Add(array);

                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                workBook.Dispose();
                throw ex;
            }

            workBook.Dispose();
            return dataTable;
        }

        public static DataTable ConvertCSVtoDataTable(Stream stream)
        {
            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(stream))
            {
                //string[] headers = sr.ReadLine().Split(',');
                string firstLine = sr.ReadLine();

                string[] headers = csvParser.Split(firstLine);
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                int index = 0;
                try
                {
                    while (!sr.EndOfStream)
                    {
                        //if (index == 653)
                        //{
                        //    string dd = "adf";
                        //}
                        //string[] rows = sr.ReadLine().Split(',');
                        string line = sr.ReadLine().Replace("\"", "");
                        if (string.IsNullOrEmpty(line) == true)
                            continue;

                        string[] rows = csvParser.Split(line);
                        if (rows.Length != dt.Columns.Count)
                            continue;

                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                        index += 1;
                    }
                }
                catch (Exception ex)
                {
                    int i = index;
                }
            }

            return dt;
        }
    }
}
