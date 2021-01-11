using System;
using System.IO;

namespace DyspaceWork
{
    public class StaticActivityGift: DataRow
    {
        /// <summary> 主键 </summary>
        public Int32 id
        {
            get;
            private set;
        }
        /// <summary> 领取次数 </summary>
        public Int32 award_count
        {
            get;
            private set;
        }
        /// <summary> 掉落id </summary>
        public Int16 drop_id
        {
            get;
            private set;
        }
        /// <summary> 等待时间（min） </summary>
        public Int64 time_min
        {
            get;
            private set;
        }
        /// <summary> 等待时间（max） </summary>
        public Int64 time_max
        {
            get;
            private set;
        }
        public override void ReadRowData(BinaryReader reader)
        {
            id = reader.ReadInt32() ;
            award_count = reader.ReadInt32() ;
            drop_id = reader.ReadInt16() ;
            time_min = reader.ReadInt64() ;
            time_max = reader.ReadInt64() ;
        }
        public override int GetPk()
        {
            return id;
        }
    }
}
