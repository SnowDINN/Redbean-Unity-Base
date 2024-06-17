using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetUserProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetUserRequest(args[0]);
			if (request.Code > 0)
				return request;

			var response = request.ToConvert<Dictionary<string, object>>();
			var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response["token"].ToString());
			if (tokenResponse != null)
				ApiBase.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

			var user = this.GetModel<UserModel>();
			var userResponse = JsonConvert.DeserializeObject<UserResponse>(response["user"].ToString());
			if (userResponse != null)
			{
				user.Response = userResponse;
				user.ModelPublish().SetPlayerPrefs();
			}

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return request;
		}
	}
}