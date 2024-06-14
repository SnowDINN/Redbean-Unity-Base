using System.Threading.Tasks;
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

			var response = request.ToConvert<UserResponse>();

			this.GetModel<UserModel>().Response = response;
			this.GetModel<UserModel>().Publish().SetPlayerPrefs();
			this.Publish(request);
			
			Log.Print($"Login user's data. [ {response.Information.Nickname} | {response.Social.Id} ]");
			return request;
		}
	}
}