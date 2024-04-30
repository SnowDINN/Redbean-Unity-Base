using System.Collections.Generic;
using Redbean.Firebase;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		public static void PostFirebase(this IDictionary<string, object> document) =>
			FirebaseCore.UserDB.UpdateAsync(document);

		public static void PostFirebase(this IRxModel rxModel, string key) =>
			FirebaseCore.UserDB.UpdateAsync(new Dictionary<string, object> { { key, rxModel } });
	}
}