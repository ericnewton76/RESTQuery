using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RESTQuery
{
	public class FilterOptions
	{

		/// <summary>
		/// Construct a new FilterOptions instance
		/// </summary>
		/// <param name="field">field to filter</param>
		/// <param name="operator">operator</param>
		/// <param name="value">value</param>
		public FilterOptions(string field, Operator @operator, string value)
		{
			if(string.IsNullOrEmpty(field)) throw new ArgumentException("field is null or empty.");
			if (@operator == Operator.Unspecified) throw new ArgumentException("operator is unspecified.");

			this.Field = field;
			this.Operator = @operator;
			this.FilterValue = value;
		}

		public string Field { get; private set; }
		public Operator Operator { get; private set; }
		public string FilterValue { get; set; }

		public bool Operator_IsFn()
		{
			switch(this.Operator)
			{
				case Operator.StartsWith: return true;
				case Operator.EndsWith: return true;
				case Operator.Contains: return true;

				default:
					return false;
			}
		}

		public string Operator_GetOp()
		{
			switch(this.Operator)
			{
				case Operator.Equal: return "=";
				case Operator.NotEqual: return "!=";
				case Operator.GreaterThan: return ">";
				case Operator.LessThan: return "<";
				default:
					return null;
			}
		}
		

	}

	public enum Operator
	{
		Unspecified,
		LessThan,
		GreaterThan,
		Equal,
		NotEqual,
		Exists,
		DoesNotExist,
		Before,
		After,
		StartsWith,
		Contains,
		EndsWith,
		//IsNull,
		//IsNotNull,
		Regex
	}
}