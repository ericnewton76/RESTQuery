
using System.Runtime.Serialization;

namespace RESTQuery
{

	[DataContract]
	public class PagingOptions
	{
		[DataMember(Name="start", EmitDefaultValue=true)]
		public int Start { get; set; }
		[DataMember(Name="rows", EmitDefaultValue=true)]
		public int Rows { get; set; }
	}

}