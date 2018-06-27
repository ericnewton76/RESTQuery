using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTQuery
{
	public class SortOptionsParser
	{

		/// <summary>
		/// Try to parse a sort value into a SortOption array
		/// </summary>
		/// <param name="sort"></param>
		/// <param name="sortOptions"></param>
		/// <returns>true if parsing succeeded, false if not.</returns>
		public bool TryParse(string sort, out SortOption[] sortOptions)
		{
			try
			{
				sortOptions = Parse(sort);
				return true;
			}
			catch
			{
				sortOptions = new SortOption[0];
				return false;
			}
		}

		/// <summary>
		/// Parses a sort querystring value into a SortOption array
		/// </summary>
		/// <param name="sort"></param>
		/// <returns></returns>
		public SortOption[] Parse(string sort)
		{
			List<SortOption> list = new List<SortOption>();
			try
			{
				string[] sortparts = sort.Split(',');
				foreach(var sortpart in sortparts)
				{
					string[] parts = sortpart.Split(' ');

					SortDirection dir;
					if(Enum.TryParse<SortDirection>(parts[1], out dir) == false)
						dir = SortDirection.Ascending;

					SortOption s = new SortOption(parts[0], dir);
					list.Add(s);
				}

				return list.ToArray();
			}
			catch(Exception ex)
			{
				throw new FormatException("Failed to parse sort value.", ex);
			}
		}
	}
}