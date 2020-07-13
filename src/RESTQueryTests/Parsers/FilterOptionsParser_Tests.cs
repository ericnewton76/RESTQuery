using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using RESTQuery;
using RESTQuery.Parsers;

namespace RESTQueryTests.Parsers
{
	[TestFixture]
	public class FilterOptionsParser_Tests
	{
		[Test]
		[TestCase("branchname.eq=Some Value", "branchname", "Equal", "Some Value")]
		[TestCase("branchname.neq=Some Value", "branchname", "NotEqual", "Some Value")]
		[TestCase("branchname.startswith=Some", "branchname", "StartsWith", "Some")]
		[TestCase("branchname.endswith=Value", "branchname", "EndsWith", "Value")]
		public void Parser_parts(string filterstring, string field, string op, string filterValue)
		{
			//arrange
			var parts = filterstring.Split('=');
			var filterkvp = new KeyValuePair<string, string>(parts[0], parts[1]);
			var list = new KeyValuePair<string, string>[] { filterkvp };

			//act
			var fop = new FilterOptionsParser().Parse(list).FirstOrDefault();

			//assert
			fop.Field.Should().Be(field);
			fop.Operator.ToString().Should().Be(op);
			fop.FilterValue.Should().Be(filterValue);
		}

		[Test]
		public void EmptyKvpList()
		{
			//arrange
			var list = new KeyValuePair<string, string>[0];

			//act
			var fop = new FilterOptionsParser().Parse(list);

			//assert
			fop.Should().HaveCount(0);
		}

	}
}
