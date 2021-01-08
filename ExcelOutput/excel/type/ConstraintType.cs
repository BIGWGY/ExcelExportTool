namespace ExcelTool
{
    public enum ConstraintType: byte
    {
        /// <summary>
        /// 空值
        /// </summary>
        NONE,
        
        /// <summary>
        /// 限制字段类型为枚举类型。
        /// </summary>
        Enum,
        
        /// <summary>
        /// 关联其他的表字段。
        /// </summary>
        Association,
        
        /// <summary>
        /// 字段值唯一。
        /// </summary>
        Unique,
    }
}