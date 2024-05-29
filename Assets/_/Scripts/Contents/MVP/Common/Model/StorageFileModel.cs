using Firebase.Firestore;
using Redbean.Firebase;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class StorageFileModel : IModel
	{
		[FirestoreProperty(FirebaseDefine.TableConfig)]
		public string[] Table { get; set; } = { };

		[FirestoreProperty(FirebaseDefine.BundleConfig)]
		public string[] Bundle { get; set; } = { };
	}
}