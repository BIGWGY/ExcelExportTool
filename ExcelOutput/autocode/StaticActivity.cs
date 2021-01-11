using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DyspaceWork
{
    public class StaticActivity: DataRow
    {
        /// <summary> 主键 </summary>
        public Int32 id
        {
            get;
            private set;
        }
        /// <summary> 显示顺序 </summary>
        public Int16 display_order
        {
            get;
            private set;
        }
        /// <summary> 活动类型 </summary>
        public Int16 type
        {
            get;
            private set;
        }
        /// <summary> 活动分类 </summary>
        public Int16 act_type
        {
            get;
            private set;
        }
        /// <summary> 活动显示分类 </summary>
        public Int16 show_type
        {
            get;
            private set;
        }
        /// <summary> 活动banner </summary>
        public string image
        {
            get;
            private set;
        }
        /// <summary> 活动名 </summary>
        public Int32 name
        {
            get;
            private set;
        }
        /// <summary> 标题 </summary>
        public Int32 title
        {
            get;
            private set;
        }
        /// <summary> 描述 </summary>
        public Int32 describe
        {
            get;
            private set;
        }
        /// <summary> 活动标签 </summary>
        public string icon
        {
            get;
            private set;
        }
        /// <summary> 时间是否显示 </summary>
        public byte time_display
        {
            get;
            private set;
        }
        /// <summary> 活动结算开始时间 </summary>
        public DateTime start_time_absolute
        {
            get;
            private set;
        }
        /// <summary> 活动结算结束时间 </summary>
        public DateTime end_time_absolute
        {
            get;
            private set;
        }
        /// <summary> 活动预告开始时间 </summary>
        public DateTime display_start_time_absolute
        {
            get;
            private set;
        }
        /// <summary> 活动关闭结束时间 </summary>
        public DateTime display_end_time_absolute
        {
            get;
            private set;
        }
        /// <summary> 活动结算开始时间 </summary>
        public Int64 start_time
        {
            get;
            private set;
        }
        /// <summary> 活动结算结束时间 </summary>
        public Int64 end_time
        {
            get;
            private set;
        }
        /// <summary> 活动预告开始时间 </summary>
        public Int64 display_start_time
        {
            get;
            private set;
        }
        /// <summary> 活动关闭结束时间 </summary>
        public Int64 display_end_time
        {
            get;
            private set;
        }
        /// <summary> 允许渠道 </summary>
        public string allow_channel_ids_Array1
        {
            get;
            private set;
        }
        /// <summary> 不允许渠道 </summary>
        public string filter_channel_ids_Array1
        {
            get;
            private set;
        }
        public override void ReadRowData(BinaryReader reader)
        {
            id = reader.ReadInt32() ;
            display_order = reader.ReadInt16() ;
            type = reader.ReadInt16() ;
            act_type = reader.ReadInt16() ;
            show_type = reader.ReadInt16() ;
            image = reader.ReadString() ;
            name = reader.ReadInt32() ;
            title = reader.ReadInt32() ;
            describe = reader.ReadInt32() ;
            icon = reader.ReadString() ;
            time_display = reader.ReadByte() ;
            start_time_absolute = new DateTime(reader.ReadInt64()) ;
            end_time_absolute = new DateTime(reader.ReadInt64()) ;
            display_start_time_absolute = new DateTime(reader.ReadInt64()) ;
            display_end_time_absolute = new DateTime(reader.ReadInt64()) ;
            start_time = reader.ReadInt64() ;
            end_time = reader.ReadInt64() ;
            display_start_time = reader.ReadInt64() ;
            display_end_time = reader.ReadInt64() ;
            allow_channel_ids_Array1 = reader.ReadString() ;
            filter_channel_ids_Array1 = reader.ReadString() ;
        }
        public override int GetPk()
        {
            return id;
        }
    }
}
