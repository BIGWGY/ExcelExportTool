namespace ExcelTool
{
    public class EnumInfo
    {
        /// <summary>
        /// 枚举的名字。
        /// </summary>
        private string _name;
        
        /// <summary>
        /// 枚举的值。
        /// </summary>
        private int _value;
        
        /// <summary>
        /// 文本解释。
        /// </summary>
        private string _description;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}