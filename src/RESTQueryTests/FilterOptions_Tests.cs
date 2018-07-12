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
			Assert.Throws<ArgumentNullException>(() => {
				var f = new FilterOptions(null, Operator.Equal, "");
			});
		}

	}
}
