using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Redbean.Firebase;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static UniTask CreateFirestore(this ISerializeModel model) =>
			FirebaseSetup.UserDB.SetAsync(model).AsUniTask();
		
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static UniTask CreateFirestore(this ISerializeModel model, string key) =>
			FirebaseSetup.UserDB.SetAsync(new Dictionary<string, object> { { key, model } }).AsUniTask();
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static UniTask UpdateFirestore(this ISerializeModel model, string key) =>
			FirebaseSetup.UserDB.UpdateAsync(key, model).AsUniTask();
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static UniTask UpdateFirestore<T>(this T value, string key) =>
			FirebaseSetup.UserDB.UpdateAsync(key, value).AsUniTask();

		public static async UniTask<bool> IsContainsData<T>(this T value, string collection, string path)
		{
			var equalTo = FirebaseSetup.Firestore.Collection(collection).WhereEqualTo(path, value);
			var querySnapshot = await equalTo.GetSnapshotAsync();

			return querySnapshot.Any();
		}
	}
}