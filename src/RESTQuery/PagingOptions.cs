
using System;
using System.Runtime.Serialization;

namespace RESTQuery
{

	[DataContract]
	public class PagingOptions
	{
		private int _start;
		private int _rows;

		[DataMember(Name = "start", EmitDefaultValue = true)]
		public int Start
		{
			get { return _start; }
			set
			{
				if(value < 0) throw new ArgumentOutOfRangeException(nameof(Start), "Value less than zero is invalid.");
				_start = value;
			}
		}

		[DataMember(Name="rows", EmitDefaultValue=true)]
		public int Rows
		{
			get { return this._rows; }
			set
			{
				if(value < -1) value = -1;
				this._rows = value;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if(Start < 0 || Rows < 1)
					return true;
				else
					return false;
			}
		}

	}

}