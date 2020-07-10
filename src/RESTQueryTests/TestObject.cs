using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTQueryTests
{
	public class TestObject : IEquatable<TestObject>
	{
		public int Index { get; set; }

		public bool Equals(TestObject other)
		{
			if (other == null) return false;
			if (this.Index != other.Index) return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as TestObject);
		}


		public static IEnumerable<TestObject> CreateTestObjectCollection(int count = 50)
		{
			var list = new List<TestObject>(count);

			for (int i = 0; i < count; i++)
			{
				list.Add(new TestObject { Index = i });
			}

			return list;
		}

	}

}
