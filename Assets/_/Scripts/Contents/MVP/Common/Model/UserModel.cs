using Firebase.Firestore;
using R3;
using Sirenix.OdinInspector;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class UserModel : ISerializeModel
	{
		public AuthenticationType AuthenticationType = AuthenticationType.Guest;
		public IRxModel Rx => new UserRxModel();
		
		[FirestoreProperty("id"), ShowInInspector]
		public string Id { get; set; } = string.Empty;

		[FirestoreProperty(DataKey.USER_DETAILS_KEY), ShowInInspector]
		public UserDetailsModel Details { get; set; } = new();
	}

	[FirestoreData]
	public class UserDetailsModel
	{
		[FirestoreProperty("nickname"), ShowInInspector]
		public string Nickname { get; set; } = string.Empty;
	}
	
	public class UserRxModel : IRxModel
	{
		public ReactiveProperty<string> Id = new();
		public ReactiveProperty<UserDetailsModel> Details = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not UserModel model)
				return;

			Id.Value = model.Id;
			Details.Value = model.Details;
		}
	}
}