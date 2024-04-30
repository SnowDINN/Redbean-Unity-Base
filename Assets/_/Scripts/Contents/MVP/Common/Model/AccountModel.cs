using Firebase.Firestore;
using R3;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AccountModel : ISerializeModel
	{
		[FirestoreProperty]
		public string userId { get; set; } = string.Empty;
		
		[FirestoreProperty]
		public string nickname { get; set; } = string.Empty;

		public IRxModel Rx { get; } = new AccountRxModel();
	}
	
	public class AccountRxModel : IRxModel
	{
		public ReactiveProperty<string> UserId = new();
		public ReactiveProperty<string> Nickname = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not AccountModel model)
				return;

			UserId.Value = model.userId;
			Nickname.Value = model.nickname;
		}
	}
}