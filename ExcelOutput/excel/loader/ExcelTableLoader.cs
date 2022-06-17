using System;
using System.IO;
using ikvm.extensions;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.Util;

namespace ExcelTool
{
    public class ExcelTableLoader : ExcelLoader
    {
        /// <summary>
        /// 表格数据的映射对象。
        /// </summary>
        private ExcelDataTable _excelDataTable;

        private string _filepath;

        private string _filename;

        private IWorkbook _workbook;

        public ExcelTableLoader(ExcelContext excelContext, string path, string filename)
        {
            _filepath = path;
            _filename = filename;
            _excelContext = excelContext;
            _excelDataTable = new ExcelDataTable(_filepath, _filename.Remove(_filename.LastIndexOf(".")));
        }

        public ExcelDataTable ExcelDataTable
        {
            get => _excelDataTable;
        }

        /// <summary>
        /// 读取字段信息。
        /// </summary>
        private void ReadTableColumnInfo()
        {
            ISheet mainSheet = _workbook.GetSheet(ExcelDataTable.MainSheetName);
            if (mainSheet == null)
            {
                throw new RuntimeException($"表格 {_excelDataTable.DataFileName} 配置错误!（可能有空格）");
            }
            // 前四个字段类型行数据
            IRow belongRow = mainSheet.GetRow(0);
            IRow typeRow = mainSheet.GetRow(1);
            IRow titleRow = mainSheet.GetRow(2);
            IRow nameRow = mainSheet.GetRow(3);
            
            bool primaryKey;
            ColumnInfo columnInfo;
            short startColumn = belongRow.FirstCellNum;
            short endColumn = belongRow.LastCellNum;
            
            for(short col= startColumn; col < endColumn; col++)
            {
                primaryKey = false;
                
                if ("".Equals(GetCellString(belongRow.GetCell(col))) || "*".Equals(GetCellString(nameRow.GetCell(col))) || "".Equals(GetCellString(nameRow.GetCell(col)))) // 注释列
                    continue;
                
                try
                {
                    ColumnType columnType;
                    if ("主键".Equals(GetCellString(titleRow.GetCell(col))))
                    {
                        primaryKey = true;
                        if (_excelDataTable.GetPrimaryColumnInfo() != null)
                        {
                            throw new Exception("重复主键!");
                        }

                        string typeString = GetColumnType(GetCellString(typeRow.GetCell(col))).toString().ToLower();
                        if (!typeString.Contains("int"))
                        {
                            throw new Exception($"主键只支持 int32 类型!");
                        }
                        if (!typeString.Equals("int32"))
                        {
                            AddDebugInfo(LogLevel.Debug, $"{_excelDataTable.DataFileName}, 主键默认为 int32!");
                        }
                        columnType = ColumnType.Int32;
                    }
                    else
                    {
                        columnType = GetColumnType(GetCellString(typeRow.GetCell(col)));
                    }

                    string name = GetCellString(nameRow.GetCell(col));
                    if (Word.IsCSharpKeyWord(name))
                    {
                        throw new Exception($"字段名不能为c#关键字: {name}");
                    }
                    
                    columnInfo = new ColumnInfo();
                    columnInfo.ColumnIndex = col;
                    columnInfo.PrimaryKey = primaryKey;
                    columnInfo.ColumnType = columnType;
                    columnInfo.Name = name;
                    columnInfo.Belong = GetColumnBelong(GetCellString(belongRow.GetCell(col)));
                    columnInfo.Title = GetCellString(titleRow.GetCell(col)).replaceAll("[\r\n]", "");
                    _excelDataTable.ColumnInfos.Add(columnInfo);
                }
                catch (Exception e)
                {
                    AddDebugInfo(LogLevel.Error, $"表格 {_excelDataTable.DataFileName}, 解析错误: {e.Message}");
                }
            }

            columnInfo = _excelDataTable.GetPrimaryColumnInfo();
            if (columnInfo == null)
            {
                throw new Exception($"每个表必须有一个主键: {_excelDataTable.DataFileName}");
            }
        }

        /// <summary>
        /// 返回约束类型。
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        private ConstraintType ParseConstraintType(string constraint)
        {
            if (ConstraintType.Enum.ToString().ToLower().Equals(constraint) || "枚举".Equals(constraint))
            {
                return ConstraintType.Enum;
            }
            if (ConstraintType.Association.ToString().ToLower().Equals(constraint) || "关联".Equals(constraint))
            {
                return ConstraintType.Association;
            }
            if (ConstraintType.Unique.ToString().ToLower().Equals(constraint) || "唯一值".Equals(constraint))
            {
                return ConstraintType.Unique;
            } 
            return ConstraintType.NONE;
        }
        
        /// <summary>
        /// 解析表格约束配置。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private TableConstraint ParseTableConstraint(IRow row)
        {
            if (row == null || row.Cells.Count < 2)
            {
                return null;
            }
            
            // 字段
            string columnName = GetCellString(row.GetCell(0));
            if (!_excelDataTable.HasColumnName(columnName))
            {
                // AddDebugInfo(LogLevel.Debug, $"表格 {_excelDataTable.DataFileName} 约束条件, 第 {row.RowNum + 1} 行, 字段名 {columnName} 未找到.");
                return null;
            }

            // 关系
            string constraint = GetCellString(row.GetCell(1)).ToLower();
            ConstraintType constraintType = ParseConstraintType(constraint);
            if (constraintType == ConstraintType.NONE)
            {
                // AddDebugInfo(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 约束条件, 第 {row.RowNum + 1} 行, 字段名 {columnName}, 关系 {constraint} 未找到!");
                return null;
            }

            // 参数
            string value = GetCellString(row.GetCell(2));
            if (constraintType == ConstraintType.Association)
            {
                if (value.Split('|').Length != 2)
                {
                    // AddDebugInfo(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 约束条件, 第 {row.RowNum + 1} 行, 字段名 {columnName}, 关系 {constraint} 配置错误! 格式: 关联表名|关联字段名");
                    return null;
                }
            }
         
            return TableConstraint.ValueOf(columnName, constraintType, GetCellString(row.GetCell(2)));
        }
        
        /// <summary>
        /// 读取表格约束。
        /// </summary>
        private void ReadTableConstraint()
        {
            ISheet constraintSheet = _workbook.GetSheet(ExcelDataTable.SettingSheetName);
            if (constraintSheet == null)
            {
                return;
            }
            // 默认起始行为标题行
            for (int i = 1; i <= constraintSheet.LastRowNum; i++)
            {
                IRow row = constraintSheet.GetRow(i);
                TableConstraint constraint = ParseTableConstraint(row);
                if (constraint != null)
                {
                    _excelDataTable.TableConstraints.Add(constraint);              
                }
            }
        }

        /// <summary>
        ///  读取数据.
        /// </summary>
        private void ReadData()
        {
            ISheet mainSheet = _workbook.GetSheet(ExcelDataTable.MainSheetName);
            mainSheet.ForceFormulaRecalculation = true;
            int count = 0;
            for (int i = ExcelDataTable.DataColumnStartRow; i <= mainSheet.LastRowNum; i++)
            {
                IRow row = mainSheet.GetRow(i);
                if (IsEmptyRow(row))
                {
                    continue;
                }
                foreach (var columnInfo in _excelDataTable.ColumnInfos)
                {
                    columnInfo.Values.Add(GetCellString(row.GetCell(columnInfo.ColumnIndex)));                    
                }

                count += 1;
            }

            _excelDataTable.DataRowCount = count;
        }
        
        /// <summary>
        /// 加载表格。
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        public void Load()
        {
            using (FileStream stream = new FileStream(_filepath, FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(stream);
                
                ISheet mainSheet = _workbook.GetSheet(ExcelDataTable.MainSheetName);
                if (mainSheet == null || mainSheet.PhysicalNumberOfRows < 4)
                {
                    throw new RuntimeException($"表格 {_excelDataTable.DataFileName} 配置错误! 第一个sheet的名字需要配置为 Main");
                }
                ReadTableColumnInfo();
                ReadTableConstraint();
                ReadData();
                
                _workbook.Close();
            }
        }
    }
}