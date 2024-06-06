using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Api
{
	public class ApiBase
	{
		protected async Task<string> GetApi(string uri)
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