using Firebase.Firestore;
using R3;
using Sirenix.OdinInspector;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class UserModel : ISerializeModel
	{
		public IRxModel Rx => new UserRxModel();

		[FirestoreProperty(DataKey.USER_INFORMATION_KEY), ShowInInspector]
		public UserInfoModel Information { get; set; } = new();
		
		[FirestoreProperty(DataKey.USER_SOCIAL_KEY), ShowInInspector]
		public UserSocialModel Social { get; set; } = new();
	}

	[FirestoreData]
	public class UserInfoModel
	{
		[FirestoreProperty(DataKey.USER_NICKNAME_KEY), ShowInInspector]
		public string Nickname { get; set; } = string.Empty;
	}
	
	[FirestoreData]
	public class UserSocialModel
	{
		[FirestoreProperty(DataKey.USER_ID_KEY), ShowInInspector]
		public string Id { get; set; } = string.Empty;
		
		[FirestoreProperty(DataKey.USER_PLATFORM_KEY), ShowInInspector]
		public string Platform { get; set; } = string.Empty;
	}
	
	public class UserRxModel : IRxModel
	{
		public ReactiveProperty<UserInfoModel> Information = new();
		public ReactiveProperty<UserSocialModel> Social = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not UserModel model)
				return;
			
			Information.Value = model.Information;
			Social.Value = model.Social;
		}
	}
}