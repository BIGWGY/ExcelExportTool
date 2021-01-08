namespace ExcelTool
{
    public class TableConstraint
    {
        /// <summary>
        /// 约束的表格字段名。
        /// </summary>
        private string _columnName;

        /// <summary>
        /// 约束类型。
        /// </summary>
        private ConstraintType _constraintType;

        /// <summary>
        /// 约束参数。
        /// </summary>
        private string _constraintParams;

        public string ColumnName
        {
            get => _columnName;
            set => _columnName = value;
        }

        public ConstraintType ConstraintType
        {
            get => _constraintType;
            set => _constraintType = value;
        }

        public string ConstraintParams
        {
            get => _constraintParams;
            set => _constraintParams = value;
        }

        public static TableConstraint ValueOf(string columnName, ConstraintType constraintType, string value)
        {
            TableConstraint tableConstraint = new TableConstraint();
            tableConstraint.ColumnName = columnName;
            tableConstraint.ConstraintType = constraintType;
            tableConstraint.ConstraintParams = value;
            return tableConstraint;
        }
    }
}