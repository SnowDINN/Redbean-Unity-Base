using Firebase.Firestore;
using Redbean.MVP;

namespace Redbean.Content.MVP
{
	[FirestoreData]
	public class AccountModel : IPostModel
	{
		[FirestoreProperty]
		public string nickname { get; private set; } = string.Empty;
		
		[FirestoreProperty]
		public string social { get; private set; } = string.Empty;
	}
}