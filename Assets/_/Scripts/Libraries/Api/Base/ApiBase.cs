using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

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
			var format = string.Format(uri, args.Where( _ => _ is string));
			var apiResponse = await PostApi(format, args.FirstOrDefault(_ => _ is HttpContent) as HttpContent);
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
			using var http = new HttpClient();
			
			var request = await http.GetAsync(new Uri(uri));
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Print(response);
				return response;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Print(reasonPhrase, Color.red);
			return reasonPhrase;
		}
		
		private static async Task<string> PostApi(string uri, HttpContent content = null)
		{
			using var http = new HttpClient();
			
			var request = await http.PostAsync(new Uri(uri), content);
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Print(response);
				return response;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Print(reasonPhrase, Color.red);
			return reasonPhrase;
		}
		
		private static async Task<bool> DeleteApi(string uri)
		{
			using var http = new HttpClient();
			
			var request = await http.DeleteAsync(new Uri(uri));
			if (request.StatusCode == HttpStatusCode.OK)
			{
				var response = await request.Content.ReadAsStringAsync();
				request.Dispose();
				
				Log.Print(response);
				return true;
			}

			var reasonPhrase = request.ReasonPhrase;
			request.Dispose();
			
			Log.Print(reasonPhrase, Color.red);
			return false;
		}
	}
}