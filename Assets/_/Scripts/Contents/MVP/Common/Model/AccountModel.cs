using Firebase.Firestore;
using R3;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AccountModel : ISerializeModel
	{
		[FirestoreProperty]
		public string uid { get; set; } = string.Empty;
		
		[FirestoreProperty]
		public string nickname { get; set; } = string.Empty;

		public IRxModel Rx { get; } = new AccountRxModel();
	}
	
	public class AccountRxModel : IRxModel
	{
		public ReactiveProperty<string> UID = new();
		public ReactiveProperty<string> Nickname = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not AccountModel convert)
				return;

			UID.Value = convert.uid;
			Nickname.Value = convert.nickname;
		}
	}
}