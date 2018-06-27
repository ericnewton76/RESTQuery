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
	public class QueryableExtensions_AddPaging_Tests
	{

 		[Test]
		public void Null_PagingOptions()
		{
			//arrange
			var source = new List<Object>().AsQueryable();


			//act
			var actual = QueryableExtensions.AddPaging(source, (PagingOptions)null);

			//assert
			actual.Should().BeSameAs(source);
		}

		[Test]
		public void Empty_PagingOptions()
		{
			//arrange
			var source = new List<Object>().AsQueryable();
			var pagingOptions = new PagingOptions() { Start = 0, Rows = 0 };

			//act
			var actual = QueryableExtensions.AddPaging(source, pagingOptions);

			//assert
			actual.Should().BeSameAs(source);
		}


	}
}
