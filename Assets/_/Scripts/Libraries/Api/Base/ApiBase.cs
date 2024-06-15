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
		public static readonly HttpClient Http = new()
		{
			BaseAddress = new Uri("https://localhost:44395"),
			DefaultRequestHeaders =
			{
				{ "accept", "application/json" },
			}
		};
		
		public static async Task<Response> SendGetRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string or int or float).ToArray());
			var apiResponse = await GetApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value).ToLower()]}";

			return Response.Return(apiParse[nameof(Response.Code).ToLower()].Value<int>(), apiResult);
		}
		
		public static async Task<Response> SendPostRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string or int or float).ToArray());
			var apiResponse = await PostApi(format, args.FirstOrDefault(_ => _ is HttpContent) as HttpContent);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value).ToLower()]}";
			
			return Response.Return(apiParse[nameof(Response.Code).ToLower()].Value<int>(), apiResult);
		}
		
		public static async Task<Response> SendDeleteRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where( _ => _ is string).ToArray());
			var apiResponse = await DeleteApi(format);
			
			return Response.Return(apiResponse ? 0 : 1, default);
		}
		
		private static async Task<string> GetApi(string uri)
		{
			var request = await Http.GetAsync(uri);
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
			var request = await Http.PostAsync(uri, content);
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
			var request = await Http.DeleteAsync(uri);
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