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

		public IEnumerable<TestObject> CreateTestObjectCollection(int count = 50)
		{
			return TestObject.CreateTestObjectCollection(count);
		}

		[Test]
		public void Zero_Filters()
		{
			//arrange
			var source = CreateTestObjectCollection().AsQueryable();
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
			var source = CreateTestObjectCollection().AsQueryable();
			IEnumerable<FilterOptions> filters = null;

			//act
			var actual = QueryableExtensions.AddFilters(source, filters);

			//assert
			actual.Should().BeSameAs(source);
		}

		[Test]
		public void EmptyKey()
		{
			//arrange
			var source = CreateTestObjectCollection().AsQueryable();
			var GetQueryNameValuePairs =
				new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("","")
				};

			//act
			var actual = QueryableExtensions.AddFilters(source, GetQueryNameValuePairs);

			//assert
			actual.Should().BeSameAs(source);
		}




	}
}
