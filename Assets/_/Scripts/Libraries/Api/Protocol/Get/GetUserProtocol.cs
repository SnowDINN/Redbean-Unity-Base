using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redbean.Bundle;
using Redbean.MVP.Content;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetUserProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetUserRequest(args[0], AppSettings.Version);
			if (request.Code > 0)
				return request;

			var response = request.ToConvert<Dictionary<string, object>>();
			var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response["token"].ToString());
			if (tokenResponse != null)
				ApiBase.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

			var user = this.GetModel<UserModel>();
			var userResponse = JsonConvert.DeserializeObject<UserResponse>(response["user"].ToString());
			if (userResponse != null)
				user.Response = userResponse;
			
			user.Publish().SetPlayerPrefs();
			this.Publish(request);

			await GoogleTableBootstrap.Setup();
			await BundleBootstrap.Setup();
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return request;
		}
	}
}