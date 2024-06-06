using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public class ApiGenerator
	{
		public const string Uri = "https://localhost:44395/";

		public static async void GetApiAsync()
		{
			var uri = $"{Uri}swagger/v1/swagger.json";

			var request = UnityWebRequest.Get(uri);
			await request.SendWebRequest();
			
			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				Debug.Log(request.downloadHandler.text);
			}
		}
	}
}