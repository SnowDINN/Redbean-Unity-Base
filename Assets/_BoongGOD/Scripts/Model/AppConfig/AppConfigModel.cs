using Firebase.Firestore;
using Redbean.Static;

namespace Redbean.Content.Model
{
	[FirestoreData]
	public class AppConfigModel : IModel
	{
		[FirestoreProperty]
		public AppConfigArgument android { get; set; }
		
		[FirestoreProperty]
		public AppConfigArgument ios { get; set; }
	}

	[FirestoreData]
	public class AppConfigArgument
	{
		[FirestoreProperty]
		public string version { get; set; }
	}
}