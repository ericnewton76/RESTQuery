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
	public class FilterOptions_Tests
	{

		[Test]
		public void ctor_throws_with_null()
		{
			Assert.Throws<ArgumentException>(() => {
				var f = new FilterOptions(null, Operator.Equal, "");
			});
		}

		[Test]
		public void ctor_throws_with_field_empty()
		{
			Assert.Throws<ArgumentException>(() => {
				var f = new FilterOptions("", Operator.Unspecified, "");
			});
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
