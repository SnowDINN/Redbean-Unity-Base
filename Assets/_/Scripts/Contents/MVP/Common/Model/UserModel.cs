using R3;
using Redbean.Api;

namespace Redbean.MVP.Content
{
	public class UserModel : ISerializeModel
	{
		public IRxModel Rx => new UserRxModel();
		
		public UserResponse Response;
	}
	
	public class UserRxModel : IRxModel
	{
		public ReactiveProperty<UserInfo> Information = new();
		public ReactiveProperty<UserSocial> Social = new();
		
		public void Publish(ISerializeModel value)
		{
			if (value is not UserModel model)
				return;
			
			Information.Value = model.Response.Information;
			Social.Value = model.Response.Social;
		}
	}
}