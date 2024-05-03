using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty("android")]
		public AppConfigArgument Android { get; private set; } = new();
		
		[FirestoreProperty("ios")]
		public AppConfigArgument iOS { get; private set; } = new();
	}

	[FirestoreData]
	public class AppConfigArgument
	{
		[FirestoreProperty("version")]
		public string Version { get; private set; } = string.Empty;
	}
}