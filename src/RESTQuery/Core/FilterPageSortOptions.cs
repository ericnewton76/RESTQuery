﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTQuery
{

	//TODO: create RESTQuery.core library to include these core items without the System.Net.Http dependency

	/// <summary>
	/// Combine all three for passing through layers
	/// </summary>
	public class FilterPageSortOptions
	{

		/// <summary>
		/// Gets or sets filters
		/// </summary>
		public IEnumerable<FilterOptions> Filters { get; set; }
		
		/// <summary>
		/// Gets or sets the paging options
		/// </summary>
		public PagingOptions Paging { get; set; }

		/// <summary>
		/// Gets or sets sorting options
		/// </summary>
		public IEnumerable<SortOption> Sort { get; set; }

		/// <summary>
		/// Adds a filter to filter options
		/// </summary>
		/// <param name="filterOption"></param>
		/// <returns></returns>
		public FilterPageSortOptions AddFilter(FilterOptions filterOption)
		{
			if (filterOption != null)
			{
				List<FilterOptions> filters = this.Filters as List<FilterOptions>;
				if (filters == null) { filters = this.Filters.ToList(); this.Filters = filters; }

				filters.Add(filterOption);
			}

			return this;
		}

		/// <summary>
		/// Adds a sorting criteria to the sort options
		/// </summary>
		/// <param name="sortOption"></param>
		/// <returns></returns>
		public FilterPageSortOptions AddSort(SortOption sortOption)
		{
			if (sortOption != null)
			{
				List<SortOption> sorts = this.Sort as List<SortOption>;
				if (sorts == null) { sorts = this.Sort.ToList(); this.Sort = sorts; }

				sorts.Add(sortOption);
			}

			return this;
		}

	}

}
