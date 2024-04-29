using System;
using Firebase.Firestore;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AccountModel : IPostModel
	{
		[FirestoreProperty]
		public string uid { get; set; } = string.Empty;
		
		[FirestoreProperty]
		public string nickname { get; set; } = string.Empty;

		public static AccountModel CreateUID()
		{
			var account = new AccountModel
			{
				uid = $"{Guid.NewGuid()}"
			};
			account.Publish().Save();

			return account;
		}
	}
}