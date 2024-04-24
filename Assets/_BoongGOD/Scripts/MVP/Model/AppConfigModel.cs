using Firebase.Firestore;
using Redbean.Static;

namespace Redbean.Content.Model
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty]
		public AppConfigArgument android { get; private set; }
		
		[FirestoreProperty]
		public AppConfigArgument ios { get; private set; }
	}

	[FirestoreData]
	public class AppConfigArgument
	{
		[FirestoreProperty]
		public string version { get; private set; }
	}
}