using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.Record;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ADOX;
public class ExcelHelper
{
    private static string path;
    public class x2003
    {
        #region Excel2003
        /// <summary>
        /// 将Excel文件中的数据读出到DataTable中(xls)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable ExcelToTableForXLS(string file,string tableName, OleDbConnection odc)
        {
            DataTable dt = new DataTable(tableName);
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(fs);  //2007
                //HSSFWorkbook hssfworkbook =new HSSFWorkbook(fs); //2003
                ISheet sheet = hssfworkbook.GetSheetAt(0);

                //表头
                IRow header = sheet.GetRow(sheet.FirstRowNum+2);       //字段名
                IRow dataType = sheet.GetRow(sheet.FirstRowNum);       //数据类型
                IRow desc = sheet.GetRow(sheet.FirstRowNum+1);       //字段描述

                path = odc.DataSource;

                object record;
                try
                {
                    ADODB.Connection cn = new ADODB.Connection();
                    cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path, null, null, -1);
                    cn.Execute("select * from " + tableName, out record, 0);
                    cn.Close();
                    cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path, null, null, -1);
                    cn.Execute("DROP TABLE " + tableName, out record, 0);
                    Console.WriteLine("更新表" + tableName);
                    SetTable(header, dataType, desc, tableName, cn,dt);
                    cn.Close();
                }
                catch (Exception)
                {
                    ADODB.Connection cn = new ADODB.Connection();
                    cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path, null, null, -1);
                    Console.WriteLine("创建新表" + tableName);
                    SetTable(header, dataType, desc, tableName, cn, dt);
                    cn.Close();
                }
                //List<int> columns = new List<int>();
                //for (int i = 0; i < header.LastCellNum; i++)
                //{
                //    object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                //    if (obj == null || obj.ToString() == string.Empty)
                //    {
                //        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                //        //continue;
                //    }
                //    else
                //        dt.Columns.Add(new DataColumn(obj.ToString()));
                //    columns.Add(i);
                //    string sql = "select * from " + tableName;
                //    OleDbCommand mycom = new OleDbCommand(sql, odc);
                //    OleDbDataAdapter da = new OleDbDataAdapter("select * from " + tableName, odc);
                //    DataTable ndt = new DataTable();
                //    da.Fill(ndt);
                //    //ndt.Columns.Contains(header.GetCell(i).StringCellValue);
                //    //OleDbParameter odp = GetDbParameter(header.GetCell(i), dataType.GetCell(i), desc.GetCell(i));
                //    //if (!mycom.Parameters.Contains(odp))
                //    //{
                //    //    mycom.Parameters.Add(odp);
                //    //}
                //}
                //数据
                for (int i = sheet.FirstRowNum + 3; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    for (int j = 0;j<header.LastCellNum;j++)
                    {
                        //dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                        dr[j] = sheet.GetRow(i).GetCell(j);
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        static void SetTable(IRow header, IRow dataType, IRow desc, string tableName, ADODB.Connection cn,DataTable dt)
        {
            Catalog catalog = new Catalog();
            catalog.ActiveConnection = cn;
            ADOX.Table table = new ADOX.Table();
            table.Name = tableName;
            UpdateFieldsData(header, dataType, desc, table,dt);
            ADOX.Column column = table.Columns[header.Cells[0].StringCellValue];
            column.ParentCatalog = catalog;
            table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
            catalog.Tables.Append(table);
        }
        static void UpdateFieldsData(IRow header, IRow dataType, IRow desc,Table table,DataTable dt)
        {
            for (int i=0;i< header.Cells.Count;i++)
            {
                switch (dataType.Cells[i].StringCellValue)
                {
                    case "int8":
                        //Column column = new ADOX.Column();
                        //column.Name = header.Cells[i].StringCellValue;
                        //column.Type = DataTypeEnum.adInteger;
                        //column.DefinedSize = 4;
                        //column.RelatedColumn = desc.Cells[i].StringCellValue;
                        //table.Columns.Append(column);
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adUnsignedTinyInt);
                        break;
                    case "int16":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adSmallInt);
                        break;
                    case "int32":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adInteger);
                        break;
                    case "float":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adSingle);
                        break;
                    case "double":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adDouble);
                        break;
                    case "string16":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adWChar, 16);
                        break;
                    case "string64":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adWChar, 64);
                        break;
                    case "string255":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adWChar, 255);
                        break;
                    case "string":
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adLongVarWChar);
                        break;
                    default:
                        //return new OleDbParameter(header.StringCellValue, OleDbType.Char, 64, desc.StringCellValue);
                        table.Columns.Append(header.Cells[i].StringCellValue, DataTypeEnum.adLongVarWChar);
                        break;
                }
                dt.Columns.Add(new DataColumn(header.Cells[i].StringCellValue));
            }
        }
        

        /// <summary>
        /// 获取单元格类型(xls)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
        #endregion

    }

    //public class x2007
    //{
    //    #region Excel2007
    //    /// <summary>
    //    /// 将Excel文件中的数据读出到DataTable中(xlsx)
    //    /// </summary>
    //    /// <param name="file"></param>
    //    /// <returns></returns>
    //    public static DataTable ExcelToTableForXLSX(string file)
    //    {
    //        DataTable dt = new DataTable();
    //        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    //        {
    //            XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
    //            ISheet sheet = xssfworkbook.GetSheetAt(0);

    //            //表头
    //            IRow header = sheet.GetRow(sheet.FirstRowNum);
    //            List<int> columns = new List<int>();
    //            for (int i = 0; i < header.LastCellNum; i++)
    //            {
    //                object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
    //                if (obj == null || obj.ToString() == string.Empty)
    //                {
    //                    dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
    //                    //continue;
    //                }
    //                else
    //                    dt.Columns.Add(new DataColumn(obj.ToString()));
    //                columns.Add(i);
    //            }
    //            //数据
    //            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
    //            {
    //                DataRow dr = dt.NewRow();
    //                bool hasValue = false;
    //                foreach (int j in columns)
    //                {
    //                    dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
    //                    if (dr[j] != null && dr[j].ToString() != string.Empty)
    //                    {
    //                        hasValue = true;
    //                    }
    //                }
    //                if (hasValue)
    //                {
    //                    dt.Rows.Add(dr);
    //                }
    //            }
    //        }
    //        return dt;
    //    }

        ///// <summary>
        ///// 将DataTable数据导出到Excel文件中(xlsx)
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="file"></param>
        //public static void TableToExcelForXLSX(DataTable dt, string file)
        //{
        //    XSSFWorkbook xssfworkbook = new XSSFWorkbook();
        //    ISheet sheet = xssfworkbook.CreateSheet("Main");

        //    //表头
        //    IRow row = sheet.CreateRow(0);
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ICell cell = row.CreateCell(i);
        //        cell.SetCellValue(dt.Columns[i].ColumnName);
        //    }

        //    //数据
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        IRow row1 = sheet.CreateRow(i + 1);
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            ICell cell = row1.CreateCell(j);
        //            cell.SetCellValue(dt.Rows[i][j].ToString());
        //        }
        //    }

        //    //转为字节数组
        //    MemoryStream stream = new MemoryStream();
        //    xssfworkbook.Write(stream);
        //    var buf = stream.ToArray();

        //    //保存为Excel文件
        //    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
        //    {
        //        fs.Write(buf, 0, buf.Length);
        //        fs.Flush();
        //    }
        //}

        ///// <summary>
        ///// 将DataTable数据导出到Excel文件中(csv)
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="file"></param>
        //public static void TableToExcelForCSV(DataTable dt, string file)
        //{
        //    //保存为Excel文件
        //    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
        //    {
        //        StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
        //        //数据
        //        for (int i = 1; i < dt.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                string content = dt.Rows[i][j].ToString();
        //                if (content == "")
        //                {
        //                    content = "0";
        //                }

        //                bool hasYinhao = false;
        //                if (-1 != content.IndexOf("\r") || -1 != content.IndexOf("\n"))
        //                {
        //                    hasYinhao = true;
        //                }
        //                string fmt = String.Format("{0}{1}0{2}{3}{4}", hasYinhao ? "\"" : "", "{", "}", hasYinhao ? "\"" : "", j + 1 == dt.Columns.Count ? "" : ",");
        //                sw.Write(fmt, content);
        //            }
        //            sw.WriteLine();
        //        }
        //        sw.Close();
        //    }
        //}

        ///// <summary>
        ///// 将DataTable数据导出到Excel文件中(txt)
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="file"></param>
        //public static void TableToExcelForTXT(DataTable dt, string file)
        //{
        //    //保存为Excel文件
        //    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
        //    {
        //        StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);
        //        //数据
        //        for (int i = 1; i < dt.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                string content = dt.Rows[i][j].ToString();
        //                if (content == "")
        //                {
        //                    content = "0";
        //                }

        //                bool hasYinhao = false;
        //                if (-1 != content.IndexOf("\r") || -1 != content.IndexOf("\n"))
        //                {
        //                    hasYinhao = true;
        //                }
        //                string fmt = String.Format("{0}{1}0{2}{3}{4}", hasYinhao ? "\"" : "", "{", "}", hasYinhao ? "\"" : "", j + 1 == dt.Columns.Count ? "" : "\t");
        //                sw.Write(fmt, content);
        //            }
        //            sw.WriteLine();
        //        }
        //        sw.Close();
        //    }
        //}

        ///// <summary>
        ///// 获取单元格类型(xlsx)
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <returns></returns>
        //private static object GetValueTypeForXLSX(XSSFCell cell)
        //{
        //    if (cell == null)
        //        return null;
        //    switch (cell.CellType)
        //    {
        //        case CellType.Blank: //BLANK:
        //            return null;
        //        case CellType.Boolean: //BOOLEAN:
        //            return cell.BooleanCellValue;
        //        case CellType.Numeric: //NUMERIC:
        //            return cell.NumericCellValue;
        //        case CellType.String: //STRING:
        //            return cell.StringCellValue;
        //        case CellType.Error: //ERROR:
        //            return cell.ErrorCellValue;
        //        case CellType.Formula: //FORMULA:
        //        default:
        //            return "=" + cell.CellFormula;
        //    }
        //}
        //#endregion
    }

    public static DataTable GetDataTable(string filepath,string tableName, OleDbConnection odc)
    {
        var dt = new DataTable("xls");
        //if (filepath.Last()=='s')
        //{
            dt = x2003.ExcelToTableForXLS(filepath,tableName, odc);
        //}
        //else
        //{
        //    dt = x2007.ExcelToTableForXLSX(filepath);
        //}
        return dt;
    }
}