using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty("android")]
		public AppConfigArgument Android { get; set; } = new();
		
		[FirestoreProperty("ios")]
		public AppConfigArgument iOS { get; set; } = new();
	}

	[FirestoreData]
	public class AppConfigArgument
	{
		[FirestoreProperty("version")]
		public string Version { get; set; } = string.Empty;
	}
}