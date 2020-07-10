using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RESTQuery
{
	public class FilterOptions
	{
		public FilterOptions(string field, Operator @operator, string value)
		{
			if(field == null) throw new ArgumentNullException("field");

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
		Regex
	}
}