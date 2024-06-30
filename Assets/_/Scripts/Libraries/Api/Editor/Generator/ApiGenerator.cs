using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public enum ApiType
	{
		Get,
		Post,
		Delete,
	}
	
	public class ApiGenerator
	{
		public const string Namespace = "Redbean";

		public static async Task GetApiAsync()
		{
			var uri = $"{AppSettings.ApiUri}/swagger/v1/swagger.json";

			var request = UnityWebRequest.Get(uri);
			await request.SendWebRequest();
			
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogError(request.error);
				return;
			}

			DeleteFiles($"{ApiSettings.ProtocolPath}");
			
			var api = JObject.Parse(request.downloadHandler.text);
			var apiEndpoints = api["paths"].ToObject<Dictionary<string, JObject>>();

			GenerateCSharpApiAsync(ApiType.Get, apiEndpoints.Where(_ => _.Value.ContainsKey("get")).ToArray());
			GenerateCSharpApiAsync(ApiType.Post, apiEndpoints.Where(_ => _.Value.ContainsKey("post")).ToArray());
			GenerateCSharpApiAsync(ApiType.Delete, apiEndpoints.Where(_ => _.Value.ContainsKey("delete")).ToArray());
		}

		
		/// <summary>
		/// API C# 스크립트 생성
		/// </summary>
		private static async void GenerateCSharpApiAsync(ApiType type, IReadOnlyList<KeyValuePair<string, JObject>> apis)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("using System.Threading.Tasks;");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"namespace {Namespace}.Api");
			stringBuilder.AppendLine("{");
			stringBuilder.AppendLine($"\tpublic class Api{type}Request : {nameof(ApiBase)}");
			stringBuilder.AppendLine("\t{");

			for (var idx = 0; idx < apis.Count; idx++)
			{
				var parameter = "";
				
				var jObject = apis[idx].Value[$"{type}".ToLower()].ToObject<JObject>();
				if (jObject.TryGetValue("parameters", out var parameters))
				{
					parameter = "?";
					
					var parameterList = parameters.ToObject<List<Dictionary<string, object>>>();
					for (var i = 0; i < parameterList.Count; i++)
					{
						parameter += $"{parameterList[i]["name"]}={{{i}}}";

						if (i < parameterList.Count - 1)
							parameter += "&";
					}
				}
				
				var requestType = "params object[] args";
				if (jObject.TryGetValue("requestBody", out var requests))
				{
					var content = requests.SelectToken("content").ToObject<JObject>();
					if (content.TryGetValue("application/json", out var json))
					{
						requestType = json.SelectToken("schema.$ref")
							.Value<string>()
							.Split('/')
							.Last();

						requestType += " args";
					}
				}

				var responseType = "";
				if (jObject.TryGetValue("responses", out var responses))
				{
					var content = responses.SelectToken("200.content").ToObject<JObject>();
					if (content.TryGetValue("application/json", out var json))
					{
						responseType = json.SelectToken("schema.$ref")
							.Value<string>()
							.Split('/')
							.Last();

						responseType = $"<{responseType}>";
					}
				}
			
				var requestUri = $"\"{apis[idx].Key}{parameter}\"";
				stringBuilder.AppendLine($"\t\tpublic static async Task<{nameof(Response)}{responseType}> {apis[idx].Key.Split('/').Last()}Request({requestType}) =>");
				stringBuilder.AppendLine($"\t\t\tawait Send{type}Request<{nameof(Response)}{responseType}>({requestUri}, args);");
				
				if (idx < apis.Count - 1)
					stringBuilder.AppendLine();
			}
			
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");
			
			if (Directory.Exists(ApiSettings.ProtocolPath))
				Directory.CreateDirectory(ApiSettings.ProtocolPath);
				
			await File.WriteAllTextAsync($"{ApiSettings.ProtocolPath}/Api{type}Request.cs", $"{stringBuilder}");
		}

		private static void DeleteFiles(string path)
		{
			var directory = new DirectoryInfo(path);
			foreach (var file in directory.GetFiles())
				file.Delete();
		}
	}
}