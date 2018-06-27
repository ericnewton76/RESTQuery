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
	public class QueryableExtensions_AddFilters_Tests
	{

		[Test]
		public void Zero_Filters()
		{
			//arrange
			var source = new List<Object>().AsQueryable();
			IEnumerable<FilterOptions> filters = new List<FilterOptions>();

			//act
			var actual = QueryableExtensions.AddFilters(source, filters);

			//assert
			actual.Should().BeSameAs(source);
		}

		[Test]
		public void Null_Filters()
		{
			//arrange
			var source = new List<Object>().AsQueryable();
			IEnumerable<FilterOptions> filters = null;

			//act
			var actual = QueryableExtensions.AddFilters(source, filters);

			//assert
			actual.Should().BeSameAs(source);
		}




	}
}
