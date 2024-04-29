using System.Collections.Generic;
using Redbean.Firebase;

namespace Redbean
{
	public static partial class Extension
	{
		public static void PostFirebase(this IDictionary<string, object> document) =>
			FirebaseCore.UserDB.UpdateAsync(document);
	}
}