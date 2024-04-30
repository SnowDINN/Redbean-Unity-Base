using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Redbean.Debug;
using Redbean.Firebase;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		public static UniTask CreateFirestore(this ISerializeModel model, string key) =>
			FirebaseCore.UserDB.SetAsync(new Dictionary<string, object> { { key, model } }).AsUniTask();

		public static UniTask UpdateFirestore(this ISerializeModel model, string key) =>
			FirebaseCore.UserDB.UpdateAsync(new Dictionary<string, object> { { key, model } }).AsUniTask();
	}
}