using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace GoodByeZ
{
	public partial class DRActivity: DataRowBase
	{
		/// <summary> 主键 </summary>
		private int m_Id = 0;
		public override int Id
		{
			get { return m_Id; }
		}

		/// <summary> 显示顺序 </summary>
		public Int16 DisplayOrder
		{
			get;
			private set;
		}

		/// <summary> 活动类型 </summary>
		public Int16 Type
		{
			get;
			private set;
		}

		/// <summary> 活动分类 </summary>
		public Int16 ActType
		{
			get;
			private set;
		}

		/// <summary> 活动显示分类 </summary>
		public Int16 ShowType
		{
			get;
			private set;
		}

		/// <summary> 活动banner </summary>
		public string Image
		{
			get;
			private set;
		}

		/// <summary> 活动名 </summary>
		public Int32 Name
		{
			get;
			private set;
		}

		/// <summary> 标题 </summary>
		public Int32 Title
		{
			get;
			private set;
		}

		/// <summary> 描述 </summary>
		public Int32 Describe
		{
			get;
			private set;
		}

		/// <summary> 活动标签 </summary>
		public string Icon
		{
			get;
			private set;
		}

		/// <summary> 时间是否显示 </summary>
		public byte TimeDisplay
		{
			get;
			private set;
		}

		/// <summary> 活动结算开始时间 </summary>
		public DateTime StartTimeAbsolute
		{
			get;
			private set;
		}

		/// <summary> 活动结算结束时间 </summary>
		public DateTime EndTimeAbsolute
		{
			get;
			private set;
		}

		/// <summary> 活动预告开始时间 </summary>
		public DateTime DisplayStartTimeAbsolute
		{
			get;
			private set;
		}

		/// <summary> 活动关闭结束时间 </summary>
		public DateTime DisplayEndTimeAbsolute
		{
			get;
			private set;
		}

		/// <summary> 活动结算开始时间 </summary>
		public Int64 StartTime
		{
			get;
			private set;
		}

		/// <summary> 活动结算结束时间 </summary>
		public Int64 EndTime
		{
			get;
			private set;
		}

		/// <summary> 活动预告开始时间 </summary>
		public Int64 DisplayStartTime
		{
			get;
			private set;
		}

		/// <summary> 活动关闭结束时间 </summary>
		public Int64 DisplayEndTime
		{
			get;
			private set;
		}

		/// <summary> 允许渠道 </summary>
		public string AllowChannelIdsArray1
		{
			get;
			private set;
		}

		/// <summary> 不允许渠道 </summary>
		public string FilterChannelIdsArray1
		{
			get;
			private set;
		}

		public virtual bool ParseDataRow(string dataRowString, object userData)
		{
			 throw new Exception("not allow call this method!");
		}

		public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
		{
			using (MemoryStream memoryStream = new MemoryStream(dataRowBytes))
			using (BinaryReader reader = new BinaryReader(memoryStream))
			{
				 try
				 {
					m_Id = reader.ReadInt32() ;
					 DisplayOrder = reader.ReadInt16() ;
					 Type = reader.ReadInt16() ;
					 ActType = reader.ReadInt16() ;
					 ShowType = reader.ReadInt16() ;
					 Image = reader.ReadString() ;
					 Name = reader.ReadInt32() ;
					 Title = reader.ReadInt32() ;
					 Describe = reader.ReadInt32() ;
					 Icon = reader.ReadString() ;
					 TimeDisplay = reader.ReadByte() ;
					 StartTimeAbsolute = new DateTime(reader.ReadInt64()) ;
					 EndTimeAbsolute = new DateTime(reader.ReadInt64()) ;
					 DisplayStartTimeAbsolute = new DateTime(reader.ReadInt64()) ;
					 DisplayEndTimeAbsolute = new DateTime(reader.ReadInt64()) ;
					 StartTime = reader.ReadInt64() ;
					 EndTime = reader.ReadInt64() ;
					 DisplayStartTime = reader.ReadInt64() ;
					 DisplayEndTime = reader.ReadInt64() ;
					 AllowChannelIdsArray1 = reader.ReadString() ;
					 FilterChannelIdsArray1 = reader.ReadString() ;
					 return true;
				 }
				 catch (Exception e)
				 {
					 Console.WriteLine(e);
					 return false;
				 }
			}
		}
	}
}
