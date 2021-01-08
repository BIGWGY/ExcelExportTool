using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DyspaceWork
{
    public class StaticVipTemplate: DataRow
    {
        /// <summary> 主键 </summary>
        public Int32 id
        {
            get;
            private set;
        }
        /// <summary> VIP等级 </summary>
        public Int32 vip_lv
        {
            get;
            private set;
        }
        /// <summary> VIP经验 </summary>
        public Int32 vip_exp
        {
            get;
            private set;
        }
        /// <summary> 立刻完成时间 </summary>
        public Int32 finish_time
        {
            get;
            private set;
        }
        /// <summary> 体力领取额外体力 </summary>
        public Int16 extra_strength
        {
            get;
            private set;
        }
        /// <summary> 每天可以使用冒险之心次数 </summary>
        public Int16 use_strength
        {
            get;
            private set;
        }
        /// <summary> 每日基础基础PVP战斗次数 </summary>
        public Dictionary<int, int> pvp_num
        {
            get;
            private set;
        }
        /// <summary> 其他效果 </summary>
        public List<int> value_Array1
        {
            get;
            private set;
        }
        public override void ReadRowData(BinaryReader reader)
        {
            id = reader.ReadInt32() ;
            vip_lv = reader.ReadInt32() ;
            vip_exp = reader.ReadInt32() ;
            finish_time = reader.ReadInt32() ;
            extra_strength = reader.ReadInt16() ;
            use_strength = reader.ReadInt16() ;
            pvp_num = ParseIntIntDictionary(reader.ReadString()) ;
            value_Array1 = reader.ReadString().Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList() ;
        }
        public override int GetPk()
        {
            return id;
        }
    }
}