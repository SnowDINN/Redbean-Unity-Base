using Firebase.Firestore;
using Redbean.Static;

namespace Redbean.Content.Model
{
	[FirestoreData]
	public class AccountModel : IModel, IPostModel
	{
		[FirestoreProperty]
		public string nickname { get; private set; }
		
		[FirestoreProperty]
		public string social { get; private set; }
	}
}