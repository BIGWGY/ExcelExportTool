using System.Collections.Generic;
using ikvm.extensions;
using java.lang;
using Microsoft.Extensions.Logging;

namespace ExcelTool
{
    public class ExcelDataTableCheck
    {
        private ExcelContext _excelContext;
        
        private ExcelDataTable _excelDataTable;
        
        public ExcelDataTableCheck(ExcelContext excelContext, ExcelDataTable excelDataTable)
        {
            _excelContext = excelContext;
            _excelDataTable = excelDataTable;
        }
        
        /// <summary>
        /// 根据字段名返回字段类。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private ColumnInfo GetColumnInfo(string columnName)
        {
            foreach (var info in _excelDataTable.ColumnInfos)
            {
                if (info.Name == columnName)
                {
                    return info;
                }
            }

            return null;
        }

        /// <summary>
        /// 返回指定类型的约束。
        /// </summary>
        /// <param name="constraintType"></param>
        /// <returns></returns>
        private List<TableConstraint> GetTableConstraint(ConstraintType constraintType)
        {
            List<TableConstraint> list = new List<TableConstraint>();

            foreach (var constraint in _excelDataTable.TableConstraints)
            {
                if (constraint.ConstraintType == constraintType)
                {
                    list.Add(constraint);
                }
            }

            return list;
        }

        /// <summary>
        /// 检查列是否有重复值。
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="column"></param>
        /// <param name="allowEmptyString"></param>
        /// <returns></returns>
        private int CheckColumnUniqueValue(string columnName, int column, bool allowEmptyString)
        {
            HashSet<string> keySet = new HashSet<string>();

            int error = 0;
            for (int i = 0; i < _excelDataTable.DataRowCount; i++)
            {
                string value = _excelDataTable.GetColumnValue(i, column);

                if (!allowEmptyString && value.isEmpty())
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName}, {columnName}列,  {i + 1} 行, 不能为空");
                    continue;
                }

                if (allowEmptyString && value.isEmpty())
                {
                    continue;
                }

                if (keySet.Contains(value))
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName},  {columnName} 列, {i + 1} 行, 重复键值: {value}");
                    continue;
                }

                keySet.Add(value);
            }

            return error;
        }

        /// <summary>
        /// 检查主键是否有重复。
        /// </summary>
        /// <returns></returns>
        public int CheckPrimaryKey()
        {
            ColumnInfo primaryColumnsInfo = _excelDataTable.GetPrimaryColumnInfo();
            if (primaryColumnsInfo == null)
            {
                return 0;
            }
            return CheckColumnUniqueValue(primaryColumnsInfo.Name, primaryColumnsInfo.ColumnIndex, false);
        }

        /// <summary>
        /// 检查字段唯一值条件。
        /// </summary>
        /// <returns></returns>
        private int CheckUniqueConstraint()
        {
            List<TableConstraint> uniqueConstraints = GetTableConstraint(ConstraintType.Unique);

            int error = 0;

            foreach (var constraint in uniqueConstraints)
            {
                ColumnInfo info = GetColumnInfo(constraint.ColumnName);
                if (info == null)
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 约束字段 {constraint.ColumnName} 不存在!");
                    continue;
                }
                error += CheckColumnUniqueValue(info.Name, info.ColumnIndex, !constraint.ConstraintParams.Contains("not null"));
            }

            return error;
        }

        /// <summary>
        /// 检查枚举字段列。
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnIndex"></param>
        /// <param name="excelEnum"></param>
        /// <returns></returns>
        private int CheckColumnEnumsValue(string columnName, int columnIndex, ExcelEnum excelEnum)
        {
            int error = 0;
            string value = "";
            for (int i = 0; i < _excelDataTable.DataRowCount; i++)
            {
                try
                {
                    value = _excelDataTable.GetColumnValue(i, columnIndex);
                    if (!excelEnum.HasEnumValue(Integer.parseInt(value)))
                    {
                        error += 1;
                        throw new Exception($"未发现枚举值 {value}");
                    }             
                }
                catch (Exception e)
                {
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} {columnName} 列, {i+1} 行, 配置错误: {e.getMessage()}");
                }
            }

            return error;
        }

        /// <summary>
        /// 检查枚举约束。
        /// </summary>
        /// <returns></returns>
        public int CheckEnumConstraint()
        {
            List<TableConstraint> enumConstraints = GetTableConstraint(ConstraintType.Enum);

            int error = 0;

            foreach (var constraint in enumConstraints)
            {
                ColumnInfo info = GetColumnInfo(constraint.ColumnName);
                if (info == null)
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 枚举约束字段 {constraint.ColumnName} 不存在!");
                    continue;
                }
                if (!_excelContext.EnumDictionary.ContainsKey(constraint.ConstraintParams))
                {
                    error += 1;
                    _excelDataTable.TableConstraints.Remove(constraint);
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 枚举约束类 {constraint.ConstraintParams} 不存在!");
                    continue;
                }

                error += CheckColumnEnumsValue(info.Name, info.ColumnIndex, _excelContext.EnumDictionary[constraint.ConstraintParams]);
            }

            return error;
        }

        /// <summary>
        /// 检查关联表的值是否存在。
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnIndex"></param>
        /// <param name="associateTable"></param>
        /// <param name="associateColumnName"></param>
        /// <returns></returns>
        private int CheckAssociateTableValues(string columnName, int columnIndex, ExcelDataTable associateTable, string associateColumnName)
        {
            int associateColumnIndex = associateTable.GetColumnIndex(associateColumnName);
            if (associateColumnIndex == -1)
            {
                return 0;
            }

            int error = 0;
            
            for (int i = ExcelDataTable.DataColumnStartRow; i < _excelDataTable.DataRowCount; i++)
            {
                string value = _excelDataTable.GetColumnValue(i, columnIndex);
                if ("".Equals(value))
                {
                    continue;
                }
                if (!associateTable.ExistColumnValue(associateColumnIndex, value))
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} {columnName} 列, {i + 1} 行, 字段 {columnName} 关联表 {associateTable.DataFileName} 的值 {value} 不存在!");
                }
            }

            return error;
        }
        
        /// <summary>
        /// 检查表关联约束。
        /// </summary>
        /// <returns></returns>
        public int CheckAssociationConstraint()
        {
            List<TableConstraint> tableConstraints = GetTableConstraint(ConstraintType.Association);

            int error = 0;

            foreach (var constraint in tableConstraints)
            {
                ColumnInfo info = GetColumnInfo(constraint.ColumnName);
                if (info == null)
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 枚举约束字段 {constraint.ColumnName} 不存在!");
                    continue;
                }

                string[] values = constraint.ConstraintParams.Split('|');
                string associateTableName = values[0];
                string associateColumn = values[1];
                if (!_excelContext.ExcelDataTables.ContainsKey(associateTableName))
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 关联表 {associateTableName} 不存在!");
                    continue;
                }

                ExcelDataTable associateTable = _excelContext.ExcelDataTables[associateTableName];
                if (!associateTable.HasColumnName(associateColumn))
                {
                    error += 1;
                    _excelContext.WriteLog(LogLevel.Error, $"表格 {_excelDataTable.DataFileName} 关联表 {associateTableName} 字段 {associateColumn} 不存在!");
                    continue;
                }

                error += CheckAssociateTableValues(constraint.ColumnName, info.ColumnIndex, associateTable, associateColumn);
            }

            return error;
        }

        /// <summary>
        /// 检查表格配置.
        /// </summary>
        /// <returns></returns>
        public int CheckDataTable()
        {
            int error = 0;
            error += CheckPrimaryKey();
            error += CheckEnumConstraint();
            error += CheckUniqueConstraint();
            error += CheckAssociationConstraint();
            _excelDataTable.Error = error;
            return error;
        }
    }
}