using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Redbean.Api
{
	public class ApiBase
	{
		private static readonly HttpClient http = new()
		{
			BaseAddress = new Uri("https://localhost:44395")
		};
		
		public static async Task<Response> SendGetRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string).ToArray());
			var apiResponse = await GetApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value)]}";
			
			return new Response(apiResult, apiParse[nameof(Response.Code)].Value<int>());
		}
		
		public static async Task<Response> SendPostRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string or int).ToArray());
			var apiResponse = await PostApi(format, args.FirstOrDefault(_ => _ is HttpContent) as HttpContent);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value)]}";
			
			return new Response(apiResult, apiParse[nameof(Response.Code)].Value<int>());
		}
		
		public static async Task<Response> SendDeleteRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string).ToArray());
			var apiResponse = await DeleteApi(format);
			
			return new Response(default, apiResponse ? 0 : 1);
		}
		
		private static async Task<string> GetApi(string uri)
		{
			var request = await http.GetAsync(uri);
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Success("GET", $"Request success : {response}");
				return response;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Fail("GET", $"Request fail : {reasonPhrase}");
			return reasonPhrase;
		}
		
		private static async Task<string> PostApi(string uri, HttpContent content = null)
		{
			var request = await http.PostAsync(uri, content);
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Success("POST", $"Request success : {response}");
				return response;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Fail("POST", $"Request fail : {reasonPhrase}");
			return reasonPhrase;
		}
		
		private static async Task<bool> DeleteApi(string uri)
		{
			var request = await http.DeleteAsync(uri);
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Success("DELETE", $"Request success : {response}");
				return true;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Fail("POST", $"Request fail : {reasonPhrase}");
			return false;
		}
	}
}