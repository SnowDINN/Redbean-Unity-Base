using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Firebase;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static Task CreateFirestore(this ISerializeModel model) =>
			FirebaseBootstrap.UserDB.SetAsync(model);
		
		/// <summary>
		/// 파이어스토어 데이터 생성
		/// </summary>
		public static Task CreateFirestore(this ISerializeModel model, string key) =>
			FirebaseBootstrap.UserDB.SetAsync(new Dictionary<string, object> { { key, model } });
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static Task UpdateFirestore(this ISerializeModel model, string key) =>
			FirebaseBootstrap.UserDB.UpdateAsync(key, model);
		
		/// <summary>
		/// 파이어스토어 데이터 업데이트
		/// </summary>
		public static Task UpdateFirestore<T>(this T value, string key) =>
			FirebaseBootstrap.UserDB.UpdateAsync(key, value);

		public static async Task<bool> IsContainsServer<T>(this T value, string collection, string path)
		{
			var equalTo = FirebaseBootstrap.Firestore.Collection(collection).WhereEqualTo(path, value);
			var querySnapshot = await equalTo.GetSnapshotAsync();

			return querySnapshot.Any();
		}
	}
}