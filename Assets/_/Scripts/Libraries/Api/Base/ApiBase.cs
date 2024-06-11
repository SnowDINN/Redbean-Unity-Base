using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public class ApiBase
	{
		public static async Task<Response> SendGetRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args);
			var apiResponse = await GetApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value)]}";
			
			return new Response(apiResult, apiParse[nameof(Response.Code)].Value<int>());
		}
		
		public static async Task<Response> SendPostRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args);
			var apiResponse = await PostApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value)]}";
			
			return new Response(apiResult, apiParse[nameof(Response.Code)].Value<int>());
		}
		
		public static async Task<Response> SendDeleteRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args);
			var apiResponse = await DeleteApi(format);
			
			return new Response(default, apiResponse ? 0 : 1);
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