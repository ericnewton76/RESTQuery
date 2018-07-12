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
	public class PagingOptions_Tests
	{

		[Test]
		[TestCase(0,1,false)]
		[TestCase(0,0,true)]
		[TestCase(1,0,true)]
		[TestCase(1,1,false)]
		public void IsEmpty(int start, int rows, bool expectingEmpty)
		{
			//arrange

			//act
			var pagingOptions = new PagingOptions()
			{
				Start = start,
				Rows = rows
			};

			//assert
			pagingOptions.IsEmpty.Should().Be(expectingEmpty);
		}

		[Test]
		public void Start_LessThan_Zero_Fails()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var pagingOptions = new PagingOptions() { Start = -1, Rows = 0 };
			});
		}

		

	}
}
