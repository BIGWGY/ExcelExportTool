namespace ExcelTool
{
    public enum ColumnBelong: byte
    {
        /// <summary>
        /// 客户端和服务器都需要的字段。
        /// </summary>
        All,
        
        /// <summary>
        /// 客户端字段。
        /// </summary>
        Client,
        
        /// <summary>
        /// 服务器字段。
        /// </summary>
        Server
    }
}