using System.Collections.Generic;
using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class TableConfigModel : IModel
	{
		[FirestoreProperty("client")]
		public TableClientArgument Client { get; set; } = new();
		
		[FirestoreProperty("sheet")]
		public TableSheetArgument Sheet { get; set; } = new();
	}
	
	[FirestoreData]
	public class TableClientArgument
	{
		[FirestoreProperty("id")]
		public string Id { get; set; } = string.Empty;
		
		[FirestoreProperty("secret")]
		public string Secret { get; set; } = string.Empty;
	}
	
	[FirestoreData]
	public class TableSheetArgument
	{
		[FirestoreProperty("id")]
		public string Id { get; set; } = string.Empty;
	}
}