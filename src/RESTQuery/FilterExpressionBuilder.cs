using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RESTQuery
{

	/// <summary>
	/// Builds Dynamic Linq Expressions 
	/// </summary>
	public class FilterExpressionBuilder
	{

		/// <summary>
		/// Builds Dynamic Linq Expressions given the filterOptions provided.
		/// </summary>
		/// <param name="filterOptions">A list of Filter Options to build a dynamic LINQ expression from</param>
		/// <returns>Tuple that has Item1 as the string, Item2 as the list of values</returns>
		public Tuple<string, List<object>> Build(IEnumerable<FilterOptions> filterOptions)
		{
			StringBuilder sb = new StringBuilder();
			List<object> values = new List<object>();
			int subvalIndex = 0;

			foreach(var filterOp in filterOptions)
			{
				if(sb.Length > 0) sb.Append(" AND ");

				if(filterOp.Operator_IsFn())
				{
					sb.Append(filterOp.Field)
						.Append('.')
						.Append(filterOp.Operator.ToString())
						.AppendFormat("(@{0})", subvalIndex++)
						.Append(" = true");

					values.Add(filterOp.FilterValue);
				}
				else
				{
					string opchar = filterOp.Operator_GetOp();

					sb.Append(filterOp.Field)
						.Append(' ')
						.Append(opchar)
						.Append(' ');

					if(true)
					{
						sb.AppendFormat("@{0}", subvalIndex++);
						values.Add(filterOp.FilterValue);
					}
				}
			}

			return new Tuple<string, List<object>>(
				sb.ToString(),
				values);
		}
	}

}