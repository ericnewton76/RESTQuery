﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Linq.Dynamic;
using System.Text;

using RESTQuery.Parsers;

namespace RESTQuery
{
	public static class QueryableExtensions
	{

		/// <summary>
		/// Applies Dynamic OrderBy clauses from the given SortOptions
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">IQueryable</param>
		/// <param name="sortOptions">enumeration of SortOptions to apply to the given IQueryable</param>
		/// <returns></returns>
		public static IQueryable<T> AddSorting<T>(this IQueryable<T> source, IEnumerable<SortOption> sortOptions)
		{
			if(sortOptions == null) return source;

			var ordered = source;
			foreach(var sortOption in sortOptions)
				ordered = System.Linq.Dynamic.DynamicQueryable.OrderBy<T>(ordered, sortOption.Field + (sortOption.Direction == SortDirection.Descending ? " desc" : " asc"));

			return ordered;
		}

		/// <summary>
		/// Applies Dynamic OrderBy clauses from the given SortOptions
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">IQueryable</param>
		/// <param name="sortOptions">enumeration of SortOptions to apply to the given IQueryable</param>
		/// <returns></returns>
		public static IQueryable<T> AddSorting<T>(this IQueryable<T> source, string sort)
		{
			if(sort == null) return source;

			var sortOptions = new SortOptionsParser().Parse(sort);

			return AddSorting(source, sortOptions);
		}

		/// <summary>
		/// Skip and Take for the given PagingOptions.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="pagingOptions"></param>
		/// <returns></returns>
		public static IQueryable<T> AddPaging<T>(this IQueryable<T> source, PagingOptions pagingOptions)
		{
			if(pagingOptions == null) return source;
			if(pagingOptions.IsEmpty) return source;

			IQueryable<T> queryable = source;

			if(pagingOptions.Start > 0)
				queryable = queryable.Skip(pagingOptions.Start);

			if(pagingOptions.Rows > 0)
				queryable = queryable.Take(pagingOptions.Rows);

			return queryable;
		}

		/// <summary>
		/// Skip and Take for the given pageIndex and pageSize parameters
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">IQueryable</param>
		/// <param name="start">starting record index</param>
		/// <param name="rows">Number of rows to return.</param>
		/// <returns></returns>
		public static IQueryable<T> AddPaging<T>(this IQueryable<T> source, int start, int rows)
		{
			var pagingOptions = new PagingOptions()
			{
				Start = start,
				Rows = rows
			};

			return AddPaging(source, pagingOptions);
		}

		/// <summary>
		/// Applies Filters 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="filterOptions"></param>
		/// <returns></returns>
		public static IQueryable<T> AddFilters<T>(this IQueryable<T> source, IEnumerable<FilterOptions> filterOptions)
		{
			if(filterOptions == null) return source;

			var exp = new FilterExpressionBuilder().Build<T>(filterOptions);
			if(exp.Item1 == "")
			{
				return source;
			}
			else
			{
				source = source.Where(exp.Item1, exp.Item2.ToArray());

				return source;
			}
		}

		/// <summary>
		/// Applies Filters given the QueryString values from Request.GetQueryNameValuePairs() method
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="GetQueryNameValuePairs">pass Request.GetQueryNameValuePairs() to this parameter</param>
		/// <returns></returns>
		public static IQueryable<T> AddFilters<T>(this IQueryable<T> source, IEnumerable<KeyValuePair<string,string>> GetQueryNameValuePairs, params string[] ignoreKeys)
		{
			if(GetQueryNameValuePairs == null) throw new ArgumentNullException(nameof(GetQueryNameValuePairs));

			var filterOptions = new FilterOptionsParser().ParseFilters(GetQueryNameValuePairs, ignoreKeys);

			return AddFilters(source, filterOptions);
		}

	}
}