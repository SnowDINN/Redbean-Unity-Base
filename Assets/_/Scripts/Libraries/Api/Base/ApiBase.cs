using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Redbean.Api
{
	public class ApiBase
	{
		protected static async Task<Response> SendGetRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where(_ => _ is string or int or float).ToArray());
			var apiResponse = await GetApi(format);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value).ToLower()]}";

			return Response.Return(apiParse[nameof(Response.Code).ToLower()].Value<int>(), apiResult);
		}
		
		protected static async Task<Response> SendPostRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where(_ => _ is string or int or float).ToArray());
			var apiResponse = await PostApi(format, args.FirstOrDefault(_ => _ is HttpContent) as HttpContent);
			var apiParse = JObject.Parse(apiResponse);
			var apiResult = $"{apiParse[nameof(Response.Value).ToLower()]}";
			
			return Response.Return(apiParse[nameof(Response.Code).ToLower()].Value<int>(), apiResult);
		}
		
		protected static async Task<Response> SendDeleteRequest(string uri, params object[] args)
		{
			var format = string.Format(uri, args.Where(_ => _ is string).ToArray());
			var apiResponse = await DeleteApi(format);
			
			return Response.Return(apiResponse ? 0 : 1, default);
		}
		
		private static async Task<string> GetApi(string uri)
		{
			var stopwatch = Stopwatch.StartNew();
			var httpUri = uri.Split('?')[0].TrimStart('/');
			
			HttpResponseMessage request = null;
			try
			{
				request = await ApiContainer.Http.GetAsync(uri);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();
					stopwatch.Stop();

					Log.Success("GET", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request success\n{response}");
					request.Dispose();

					return response;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("GET", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : {e.Message}");
				request?.Dispose();

				throw;
			}
			finally
			{
				stopwatch.Stop();
			}
			
			Log.Fail("GET", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return string.Empty;
		}
		
		private static async Task<string> PostApi(string uri, HttpContent content = null)
		{
			var stopwatch = Stopwatch.StartNew();
			var httpUri = uri.Split('?')[0].TrimStart('/');
			
			HttpResponseMessage request = null;
			try
			{
				request = await ApiContainer.Http.PostAsync(uri, content);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();
					stopwatch.Stop();
				
					Log.Success("POST", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request success\n{response}");
					request.Dispose();
					
					return response;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("POST", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : {e.Message}");
				request?.Dispose();
				
				throw;
			}
			finally
			{
				stopwatch.Stop();
			}

			Log.Fail("POST", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return string.Empty;
		}
		
		private static async Task<bool> DeleteApi(string uri)
		{
			var stopwatch = Stopwatch.StartNew();
			var httpUri = uri.Split('?')[0].TrimStart('/');
			
			HttpResponseMessage request = null;
			try
			{
				request = await ApiContainer.Http.DeleteAsync(uri);
				if (request.IsSuccessStatusCode)
				{
					var response = await request.Content.ReadAsStringAsync();
					stopwatch.Stop();
				
					Log.Success("DELETE", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request success\n{response}");
					request.Dispose();
					
					return true;
				}
			}
			catch (HttpRequestException e)
			{
				Log.Fail("DELETE", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : {e.Message}");
				request?.Dispose();
				
				throw;
			}
			finally
			{
				stopwatch.Stop();
			}
	
			Log.Fail("DELETE", $"<{httpUri}> ({stopwatch.ElapsedMilliseconds}ms) Request fail : ({(int)request.StatusCode}) {request.ReasonPhrase}");
			request.Dispose();
			
			return false;
		}
	}
}