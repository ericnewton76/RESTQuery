using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;
using RESTQuery;
using RESTQuery.Parsers;

namespace RESTQueryTests
{
	[TestFixture]
	public class FilterExpressionBuilder_Tests
	{

		[Test]
		[TestCase("branchname.eq=Some Value", "branchname = @0")]
		[TestCase("branchname.neq=Some Value", "branchname != @0")]
		[TestCase("branchname.startswith=Some", "branchname.StartsWith(@0) = true")]
		[TestCase("branchname.endswith=Value", "branchname.EndsWith(@0) = true")]
		public void BuildExpression_single(string filterstring, string expr)
		{
			//arrange
			var parts = filterstring.Split('=');
			var filterkvp = new KeyValuePair<string, string>(parts[0], parts[1]);
			var filterOps = new FilterOptionsParser().Parse(new List<KeyValuePair<string, string>>() { filterkvp });

			//act
			var exp = new FilterExpressionBuilder().Build<TestObj>(filterOps);

			//assert
			exp.Item1.Should().Be(expr);
			exp.Item2.Count().Should().Be(1);
		}

		[Test]
		[TestCase("branchname.eq=Some Value&field2.eq=X", "branchname = @0 AND field2 = @1")]
		[TestCase("branchname.neq=Some Value&field2.neq=X", "branchname != @0 AND field2 != @1")]
		[TestCase("branchname.startswith=Some&field2.startswith=X", "branchname.StartsWith(@0) = true AND field2.StartsWith(@1) = true")]
		[TestCase("branchname.endswith=Value&field2.endswith=X", "branchname.EndsWith(@0) = true AND field2.EndsWith(@1) = true")]
		public void BuildExpression_double(string filterstring, string expr)
		{
			//arrange
			var items = filterstring.Split('&').Select(_ => { var t = _.Split('='); return new KeyValuePair<string, string>(t[0], t[1]); });
			var filterOps = new FilterOptionsParser().Parse(items);

			//act
			var exp = new FilterExpressionBuilder().Build<TestObj>(filterOps);

			//assert
			exp.Item1.Should().Be(expr);
			exp.Item2.Count().Should().Be(2);
		}

		[Test]
		[TestCase("Enabled.eq=true", false, false, "2211,4411,6611,8811")]
		[TestCase("IntField.eq=1111", true, false, "1111")]
		[TestCase("IntField.neq=1111", false, true, "2211,3311,4411,5511,6611,7711,8811,9911")]
		[TestCase("IntField.gt=1199", false, true, "2211,3311,4411,5511,6611,7711,8811,9911")]
		[TestCase("IntField.lt=9900", true, false, "1111,2211,3311,4411,5511,6611,7711,8811")]
		public void DynamicExpressions_TypeConversions(string filterstring, bool shouldContain1111, bool shouldContain9911, string expectedIds)
		{
			//arrange
			var items = filterstring.Split('&').Select(_ => { var t = _.Split('='); return new KeyValuePair<string, string>(t[0], t[1]); });
			var filterOps = new FilterOptionsParser().Parse(items);

			var testObjList = CreateTestData();
			var byKeys = testObjList.ToDictionary(_ => _.IdStr);

			//act
			var actual = QueryableExtensions.AddFilters(testObjList.AsQueryable(), filterOps)
				.ToList();//run the query

			//assert
			var expectedIdsList = expectedIds.Split(',');
			actual.Should().HaveCount(expectedIdsList.Count());

			if(shouldContain1111)
				actual.Should().Contain(byKeys["1111"]);
			else
				actual.Should().NotContain(byKeys["1111"]);

			if(shouldContain9911)
				actual.Should().Contain(byKeys["9911"]);
			else
				actual.Should().NotContain(byKeys["9911"]);

			//actual.Should().ContainInOrder(expectedIdsList.ToArray());
		}

		[Test]
		[TestCase("IdStr.eq=1111", true, false, "1111")]
		[TestCase("IdStr.neq=1111&field2.neq=X", false, true, "2211,3311,4411,5511,6611,7711,8811,9911")]
		[TestCase("IdStr.neq=1111&field2.eq=Some 22 Value", false, false, "2211")]
		[TestCase("IdStr.sw=11", true, false, "1111")]
		[TestCase("IdStr.endswith=11", true, true, "1111,2211,3311,4411,5511,6611,7711,8811,9911")]
		[TestCase("IdStr.eq=2211", false, false, "2211")]
		public void DynamicExpressions(string filterstring, bool shouldContain1111, bool shouldContain9911, string expectedIds)
		{
			//arrange
			var items = filterstring.Split('&').Select(_ => { var t = _.Split('='); return new KeyValuePair<string, string>(t[0], t[1]); });
			var filterOps = new FilterOptionsParser().Parse(items);

			var testObjList = CreateTestData();
			var byKeys = testObjList.ToDictionary(_ => _.IdStr);

			//act
			var actual = QueryableExtensions.AddFilters(testObjList.AsQueryable(), filterOps)
				.ToList();//run the query

			//assert
			var expectedIdsList = expectedIds.Split(',');
			actual.Should().HaveCount(expectedIdsList.Count());

			if(shouldContain1111)
				actual.Should().Contain(byKeys["1111"]);
			else
				actual.Should().NotContain(byKeys["1111"]);

			if(shouldContain9911)
				actual.Should().Contain(byKeys["9911"]);
			else
				actual.Should().NotContain(byKeys["9911"]);

			//actual.Should().ContainInOrder(expectedIdsList.ToArray());
		}

		public static List<TestObj> CreateTestData()
		{
			var list = new List<TestObj>();

			for(int i=1; i <= 9; i++)
			{
				var x = new TestObj() {
					IdStr = new string((char)(i + 48), 2) + "11",
					Field2 = "Some " + new string((char)(i + 48), 2) + " Value",
					Enabled = (i % 2 == 0)
				};
				list.Add(x);
			};

			return list;
		}

		public class TestObj
		{
			public string IdStr { get; set; }
			public string Field2 { get; set; }
			public bool Enabled { get; set; }

			public int IntField { get { return int.Parse(IdStr); } }

			public override string ToString()
			{
				return "id:" + IdStr + "=" + Field2;
			}
			public override bool Equals(object obj)
			{
				var y = obj as TestObj;
				if(y == null) return false;
				if(this.IdStr.Equals(y.IdStr) == false) return false;
				if(this.Field2.Equals(y.Field2) == false) return false;
				if(this.Enabled.Equals(y.Enabled) == false) return false;
				return true;
			}
		}


	}
}
