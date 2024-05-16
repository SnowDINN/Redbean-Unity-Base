using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Redbean.Firebase;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		public static UniTask CreateFirestore(this SerializeModel model) =>
			FirebaseCore.UserDB.SetAsync(model).AsUniTask();
		
		public static UniTask CreateFirestore(this SerializeModel model, string key) =>
			FirebaseCore.UserDB.SetAsync(new Dictionary<string, object> { { key, model } }).AsUniTask();
		
		public static UniTask UpdateFirestore(this SerializeModel model, string key) =>
			FirebaseCore.UserDB.UpdateAsync(key, model).AsUniTask();
		
		public static UniTask UpdateFirestore<T>(this T value, string key) =>
			FirebaseCore.UserDB.UpdateAsync(key, value).AsUniTask();
	}
}