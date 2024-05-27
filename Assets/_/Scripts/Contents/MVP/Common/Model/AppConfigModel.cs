using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty("android")]
		public MobileConfigArgument Android { get; set; } = new();
		
		[FirestoreProperty("ios")]
		public MobileConfigArgument iOS { get; set; } = new();
		
		[FirestoreProperty("table")]
		public TableConfigArgument Table { get; set; } = new();
	}

	[FirestoreData]
	public class MobileConfigArgument
	{
		[FirestoreProperty("version")]
		public string Version { get; set; } = string.Empty;
	}
	
	[FirestoreData]
	public class TableConfigArgument
	{
		[FirestoreProperty("uri")]
		public string Uri { get; set; } = string.Empty;
		
		[FirestoreProperty("gid")]
		public string Gid { get; set; } = string.Empty;
	}
}