using System;
using Firebase.Firestore;
using R3;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class UserModel : SerializeModel
	{
		public AuthenticationType AuthenticationType = AuthenticationType.Guest;
		public UserModel() => Rx = new UserRxModel();
		
		[FirestoreProperty(LocalKey.USER_ID_KEY)]
		public string UserId { get; set; } = string.Empty;

		[FirestoreProperty(LocalKey.USER_INFO_KEY)]
		public UserInfoModel UserInfo { get; set; } = new();
	}

	[FirestoreData]
	public class UserInfoModel
	{
		[FirestoreProperty("nickname")]
		public string Nickname { get; set; } = string.Empty;
	}
	
	public class UserRxModel : RxModel
	{
		public ReactiveProperty<string> UserId = new();
		public ReactiveProperty<UserInfoModel> UserInfo = new();
		
		public override void Publish(SerializeModel value)
		{
			if (value is not UserModel model)
				return;

			UserId.Value = model.UserId;
			UserInfo.Value = model.UserInfo;
		}
	}
}