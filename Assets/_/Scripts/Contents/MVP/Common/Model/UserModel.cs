using R3;
using Redbean.Api;

namespace Redbean.MVP.Content
{
	public class UserModel : UserResponse, ISerializeModel
	{
		public IRxModel Rx => new UserRxModel();

		public UserModel()
		{
		}

		public UserModel(UserAndTokenResponse response)
		{
			Information = response.Information;
			Social = response.Social;
		}
	}
	
	public class UserRxModel : IRxModel
	{
		public ReactiveProperty<UserInfo> Information = new();
		public ReactiveProperty<UserSocial> Social = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not UserModel model)
				return;
			
			Information.Value = model.Information;
			Social.Value = model.Social;
		}
	}
}