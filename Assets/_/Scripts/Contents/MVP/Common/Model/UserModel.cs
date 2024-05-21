using Firebase.Firestore;
using R3;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class UserModel : ISerializeModel
	{
		public AuthenticationType AuthenticationType = AuthenticationType.Guest;
		public IRxModel Rx => new UserRxModel();
		
		[FirestoreProperty("id")]
		public string UserId { get; set; } = string.Empty;

		[FirestoreProperty(DataKey.USER_INFO_KEY)]
		public UserInfoModel UserInfo { get; set; } = new();
	}

	[FirestoreData]
	public class UserInfoModel
	{
		[FirestoreProperty("nickname")]
		public string Nickname { get; set; } = string.Empty;
	}
	
	public class UserRxModel : IRxModel
	{
		public ReactiveProperty<string> UserId = new();
		public ReactiveProperty<UserInfoModel> UserInfo = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not UserModel model)
				return;

			UserId.Value = model.UserId;
			UserInfo.Value = model.UserInfo;
		}
	}
}