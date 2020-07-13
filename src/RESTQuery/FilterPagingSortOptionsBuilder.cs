using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RESTQuery.Parsers;

namespace RESTQuery
{

	/// <summary>
	/// builds FilterPageSortOptions from querystring KeyValuePairs.  
	/// </summary>
	/// <remarks>Additional methods available in RESTQuery.SystemWeb and RESTQuery.SystemNetHttp for specific handlers</remarks>
	public class FilterPageSortOptionsBuilder
	{

		private FilterPageSortOptions fps;

		/// <summary>
		/// Build a FilterPagingSortOptions instance
		/// </summary>
		/// <param name="querystring"></param>
		/// <param name="start"></param>
		/// <param name="rows"></param>
		/// <param name="ignoreFilterKeys"></param>
		/// <returns></returns>
		public FilterPageSortOptionsBuilder Parse(IEnumerable<KeyValuePair<string,string>> querystring, int start = 0, int rows = 50, params string[] ignoreFilterKeys)
		{
			this.ParseFiltering(querystring, ignoreFilterKeys);
			this.ParseSorting(querystring);
			this.ParsePaging(querystring, start, rows);

			return this;
		}

		public FilterPageSortOptionsBuilder ParseSorting(IEnumerable<KeyValuePair<string, string>> querystring)
		{
			if (this.fps == null) this.fps = new FilterPageSortOptions();

			var sortqs = querystring.Where(_ => _.Key == "sort").FirstOrDefault();
			if (sortqs.Value != null)
			{
				var sortOptions = new SortOptionsParser().Parse(querystring.First(_ => _.Key == "sort").Value);
				fps.Sort = sortOptions;
			}
			else
				fps.Sort = new SortOption[0];

			return this;
		}

		public FilterPageSortOptionsBuilder ParsePaging(IEnumerable<KeyValuePair<string, string>> querystring, int start = 0, int rows = 50)
		{
			if (this.fps == null) this.fps = new FilterPageSortOptions();

			var startqs = querystring.Where(_ => _.Key == "start").FirstOrDefault();
			if (int.TryParse(startqs.Value, out int startqsVal)) { start = startqsVal; }

			var rowsqs = querystring.FirstOrDefault(_ => _.Key == "rows");
			if (int.TryParse(rowsqs.Value, out int rowsqsVal)) { rows = rowsqsVal; }

			fps.Paging = new PagingOptions() { Start = start, Rows = rows };

			return this;
		}

		/// <summary>
		/// Use this by calling Request.GetQueryNameValuePairs() for first parameter.  This method will filter out start,rows,sort query string values.
		/// </summary>
		/// <param name="GetQueryNameValuePairs"></param>
		/// <returns></returns>
		public FilterPageSortOptionsBuilder ParseFiltering(IEnumerable<KeyValuePair<string, string>> querystring, params string[] ignoreKeys)
		{
			if (this.fps == null) this.fps = new FilterPageSortOptions();

			var qs = querystring.Where(_ =>
				_.Key.Equals("start", StringComparison.OrdinalIgnoreCase) == false
				&& _.Key.Equals("rows", StringComparison.OrdinalIgnoreCase) == false
				&& _.Key.Equals("sort", StringComparison.OrdinalIgnoreCase) == false
				&& _.Key != ""
			);

			this.fps.Filters = new FilterOptionsParser().Parse(qs, ignoreKeys);
			return this;
		}

		/// <summary>
		/// Gets the current FilterPageSortOptions instance.
		/// </summary>
		/// <returns></returns>
		public FilterPageSortOptions GetFilterPageSortOptions()
		{
			if (this.fps == null) return new FilterPageSortOptions();
			return this.fps;
		}
	}

}
