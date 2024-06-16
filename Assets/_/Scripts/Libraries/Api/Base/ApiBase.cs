using System;
using System.Linq;
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
			},
			Timeout = TimeSpan.FromSeconds(10),
			
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
			HttpResponseMessage request = null;
			try
			{
				request = await Http.GetAsync(uri);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();

					Log.Success("GET", $"Request success : {response}");
					request.Dispose();

					return response;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("GET", $"Request fail : {e.Message}");
				request?.Dispose();

				throw;
			}
			
			Log.Fail("GET", $"Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return string.Empty;
		}
		
		private static async Task<string> PostApi(string uri, HttpContent content = null)
		{
			HttpResponseMessage request = null;
			try
			{
				request = await Http.PostAsync(uri, content);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();
				
					Log.Success("POST", $"Request success : {response}");
					request.Dispose();
					
					return response;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("POST", $"Request fail : {e.Message}");
				request?.Dispose();
				
				throw;
			}

			Log.Fail("POST", $"Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return string.Empty;
		}
		
		private static async Task<bool> DeleteApi(string uri)
		{
			HttpResponseMessage request = null;
			try
			{
				request = await Http.DeleteAsync(uri);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();
				
					Log.Success("DELETE", $"Request success : {response}");
					request.Dispose();
					
					return true;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("DELETE", $"Request fail : {e.Message}");
				request?.Dispose();
				
				throw;
			}

			Log.Fail("DELETE", $"Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return false;
		}
	}
}