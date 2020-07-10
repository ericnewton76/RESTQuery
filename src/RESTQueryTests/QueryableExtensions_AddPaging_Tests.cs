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

		[Test]
		[TestCase(/*Start=*/ 0, /*Rows=*/ 5, /*FirstIndex=*/ 0)]
		[TestCase(/*Start=*/ 1, /*Rows=*/ 5, /*FirstIndex=*/ 1)]
		public void StartRows(int start, int rows, int ExpectedFirstIndex)
		{
			//arrange
			var testobjCollection = GetTestObjectCollection();
			var source = testobjCollection.AsQueryable();
			var pagingOptions = new PagingOptions() { Start = start, Rows = rows };

			//act
			var actual = QueryableExtensions.AddPaging(source, pagingOptions).ToList();

			//assert
			actual.Should().HaveElementAt(0, new TestObject { Index = ExpectedFirstIndex });
			//actual.Should().HaveElementAt(0, testobjCollection[ExpectedFirstIndex]);
		}



		[Test]
		public void Rows_Unlimited()
		{
			//arrange
			var testobjCollection = GetTestObjectCollection(999);
			var source = testobjCollection.AsQueryable();
			var pagingOptions = new PagingOptions() { Start = 0, Rows = -1 };

			//act
			var actual = QueryableExtensions.AddPaging(source, pagingOptions);

			//assert
			actual.Should().HaveCount(999);
		}

		List<TestObject> GetTestObjectCollection(int count = 50)
		{
			var list = new List<TestObject>(count);

			for(int i = 0; i < count; i++)
			{
				list.Add(new TestObject { Index = i });
			}

			return list;
		}

		[Test]
		public void TestObject_Equals()
		{
			//arrange
			var testobj1 = new TestObject { Index = 0 };

			//act
			var testobj2 = new TestObject { Index = 0 };

			//assert
			testobj2.Should().NotBeSameAs(testobj1);
			testobj2.Should().BeEquivalentTo(testobj1);
			testobj2.Should().Be(testobj1);
		}

	}
}
