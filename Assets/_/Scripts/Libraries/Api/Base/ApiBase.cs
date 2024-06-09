using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public class ApiBase
	{
		public static async Task<ResponseResult> SendGetRequest(string uri, params object[] parameters)
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
		
		public static async Task<ResponseResult> SendPostRequest(string uri, params object[] parameters)
		{
			var format = string.Format(uri, parameters);
			var apiResponse = await DeleteApi(format);
			return new ResponseResult
			{
				StatusCode = apiResponse ? 0 : 1,
				Result = default
			};
		}
		
		public static async Task<ResponseResult> SendDeleteRequest(string uri, params object[] parameters)
		{
			var format = string.Format(uri, parameters);
			var apiResponse = await DeleteApi(format);
			return new ResponseResult
			{
				StatusCode = apiResponse ? 0 : 1,
				Result = default
			};
		}
		
		private static async Task<string> GetApi(string uri)
		{
			var request = UnityWebRequest.Get(uri);
			await request.SendWebRequest();

			if (!string.IsNullOrEmpty(request.error))
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
		
		private static async Task<string> PostApi(string uri)
		{
			var request = UnityWebRequest.Post(uri, new Dictionary<string, string>());
			await request.SendWebRequest();

			if (!string.IsNullOrEmpty(request.error))
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
		
		private static async Task<bool> DeleteApi(string uri)
		{
			var request = UnityWebRequest.Delete(uri);
			await request.SendWebRequest();

			if (string.IsNullOrEmpty(request.error))
				return true;

			Log.Print(request.error, Color.red);
			return false;

		}
	}
}