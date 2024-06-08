using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public class ApiBase
	{
		public static async Task<ResponseResult> SendGetRequest(string uri, params string[] parameters)
		{
			var format = string.Format(uri, parameters);
			var apiResponse = await GetApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(ResponseResult.Result)]}";
			
			return new ResponseResult
			{
				StatusCode = apiParse[nameof(ResponseResult.StatusCode)].Value<int>(),
				Result = apiResult
			};
		}
		
		private static async Task<string> GetApi(string uri)
		{
			var request = UnityWebRequest.Get(uri);
			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				var message = request.error;
				Log.Print(message, Color.red);

				return message;
			}
			else
			{
				var message = request.downloadHandler.text;
				Log.Print(message);

				return message;
			}
		}
	}
}