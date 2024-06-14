using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		public static FirebaseAuth Auth => FirebaseAuth.DefaultInstance;
		public static FirebaseFirestore Firestore => FirebaseFirestore.DefaultInstance;
		
		public static DocumentReference UserDB;
		
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static Task CreateFirestore(this ISerializeModel model) =>
			UserDB.SetAsync(model);
		
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static Task CreateFirestore(this ISerializeModel model, string key) =>
			UserDB.SetAsync(new Dictionary<string, object> { { key, model } });
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static Task UpdateFirestore(this ISerializeModel model, string key) =>
			UserDB.UpdateAsync(key, model);
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static Task UpdateFirestore<T>(this T value, string key) =>
			UserDB.UpdateAsync(key, value);

		public static async Task<bool> IsContainsServer<T>(this T value, string collection, string path)
		{
			var equalTo = Firestore.Collection(collection).WhereEqualTo(path, value);
			var querySnapshot = await equalTo.GetSnapshotAsync();

			return querySnapshot.Any();
		}
	}
}