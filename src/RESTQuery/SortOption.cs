using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTQuery
{
	public enum SortDirection
	{
		Unspecified = 0,
		Ascending = 1,
		Descending = 2,
		asc = 1,
		desc = 2
	}

	public class SortOption
	{
		public SortOption(string field, SortDirection direction)
		{
			this.Field = field;
			this.Direction = direction;
		}

		public string Field { get; private set; }
		public SortDirection Direction { get; private set; }
	}

}