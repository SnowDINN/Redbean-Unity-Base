using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty]
		public AppConfigArgument android { get; private set; } = new();
		
		[FirestoreProperty]
		public AppConfigArgument ios { get; private set; } = new();
	}

	[FirestoreData]
	public class AppConfigArgument
	{
		[FirestoreProperty]
		public string version { get; private set; } = string.Empty;
	}
}