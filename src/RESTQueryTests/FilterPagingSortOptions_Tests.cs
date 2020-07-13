using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;
using RESTQuery;

namespace RESTQueryTests
{
	[TestFixture]
	public class FilterPagingSortOptions_Tests : RESTQueryTests.Parsers.ParserTests
	{

		[Test]
		public void AddFilter_works_unparsed()
		{
			var expected = new FilterOptions("fieldz", Operator.Contains, "value");

			var fps = new FilterPageSortOptions();

			fps.AddFilter("fieldz", Operator.Contains, "value");

			fps.Filters.Last().Should().BeEquivalentTo(expected);
		}

		[Test]
		public void AddFilter_works_after_parsing()
		{
			//arrange
			var expected = new FilterOptions("fieldz", Operator.Contains, "value");

			//act
			var querystring = this.GetQueryNameValuePairs("field.eq=value&fieldx.lt=value");

			var fps = new FilterPageSortOptionsBuilder()
				.Parse(querystring)
				.GetFilterPageSortOptions();

			fps.AddFilter("fieldz", Operator.Contains, "value");

			//assert
			fps.Filters.Last().Should().BeEquivalentTo(expected);
		}

		//[Test]
		public void Test_parsing()
		{
			//arrange
			var expected = new FilterOptions[] {
				new FilterOptions("field", Operator.Equal, "value"),
				new FilterOptions("fieldx", Operator.LessThan, "2"),
				new FilterOptions("fieldz", Operator.Contains, "value")
			};

			//act
			var querystring = this.GetQueryNameValuePairs("field.eq=value&fieldx.lt=2");

			var fps = new FilterPageSortOptionsBuilder()
				.Parse(querystring)
				.GetFilterPageSortOptions();

			fps.AddFilter("fieldz", Operator.Contains, "value");

			//assert
			fps.Filters.Should().BeEquivalentTo(expected);
		}

		[Test]
		public void ctor_throws_with_operator_unspecified()
		{
			Assert.Throws<ArgumentException>(() => {
				var f = new FilterOptions("someField", Operator.Unspecified, "");
			});
		}


	}
}
