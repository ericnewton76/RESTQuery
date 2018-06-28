using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace RESTQuery
{
	public class FilterOptionsParser
	{

		/// <summary>
		/// Use this by calling Request.GetQueryNameValuePairs() for first parameter.  This method will filter out start,rows,sort query string values.
		/// </summary>
		/// <param name="GetQueryNameValuePairs"></param>
		/// <returns></returns>
		public FilterOptions[] ParseFilters(IEnumerable<KeyValuePair<string,string>> GetQueryNameValuePairs)
		{
			var qs = GetQueryNameValuePairs.Where(_ => 
				_.Key.Equals("start", StringComparison.OrdinalIgnoreCase) == false 
				&& _.Key.Equals("rows", StringComparison.OrdinalIgnoreCase) == false 
				&& _.Key.Equals("sort", StringComparison.OrdinalIgnoreCase) == false
				&& _.Key != ""
			).ToList();

			return Parse(qs);
		}

		#region different style parsers
		private static FOP LastDotStyle(string key)
		{
			int indexOfDot = key.LastIndexOf(".");

			if(indexOfDot > -1)
			{
				return new FOP(
					key.Substring(0, indexOfDot),
					key.Substring(indexOfDot + 1)
				);
			}
			else
			{
				throw new FormatException($"Key '{key}' had no dot notation to extract operator.");
			}
		}
		private static FOP BracketStyle(string key)
		{
			int indexOfLB = key.IndexOf("[");
			int indexOfRB = key.IndexOf("]");

			if(indexOfLB > 0 && indexOfRB > 0)
			{
				return new FOP(
					key.Substring(0, indexOfLB),
					key.Substring(indexOfLB + 1, indexOfRB - indexOfLB - 1)
				);
			}
			else
			{
				throw new FormatException($"Key '{key}' had no brackets to extract operator.");
			}
		}

		private struct FOP
		{
			public FOP(string field, string op)
			{
				this.f = field;
				this.op = op;
			}
			public string f;
			public string op;
		}
		#endregion

		public FilterOptions[] Parse(IEnumerable<KeyValuePair<string,string>> qs)
		{
			List<FilterOptions> list = new List<FilterOptions>();

			foreach(var kvp in qs)
			{
				try
				{
					string key = kvp.Key;

					Func<string, FOP> styleparser = LastDotStyle;

					FOP fop = styleparser(kvp.Key);

					if(fop.f == null) throw new FormatException("Missing field name");
					if(fop.op == null) throw new FormatException("Missing filter operator");

					Operator op1;
					if(Enum.TryParse<Operator>(fop.op, true, out op1) == false)
					{
						if(TryParse_OpShorthand(fop.op, out op1) == false)
						{
							throw new FormatException(string.Format("Operator '{0}' is not recognized.  Valid operators are: {1}", fop.op, Enum.GetNames(typeof(Operator))));
						}
					}

					FilterOptions fo = new FilterOptions(fop.f, op1, kvp.Value);
					list.Add(fo);
				}
				catch(Exception ex)
				{
					throw new FormatException($"Failed to parse filter '{kvp.Key}'.", ex);
				}
			}

			return list.ToArray();
		}

		private bool TryParse_OpShorthand(string value, out Operator op)
		{
			switch(value.ToLowerInvariant())
			{
				case "eq": op = Operator.Equal; return true;
				case "neq": op = Operator.NotEqual; return true;
				case "lt": op = Operator.LessThan; return true;
				case "gt": op = Operator.GreaterThan; return true;
				case "regex": op = Operator.Regex; return true;
				case "sw": op = Operator.StartsWith; return true;
				case "ew": op = Operator.EndsWith; return true;

				default:
					op = (Operator)(-1);
					return false;
			}
		}

	}
}