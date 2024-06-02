using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Storage;

namespace Redbean.Container
{
	public class FirebaseContainer
	{
		public static FirebaseAuth Auth => FirebaseAuth.DefaultInstance;
		public static FirebaseStorage Storage => FirebaseStorage.DefaultInstance;
		public static FirebaseFirestore Firestore => FirebaseFirestore.DefaultInstance;
		
		public static DocumentReference UserDB;
	}
}