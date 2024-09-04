using System;
using System.Threading;
using System.Threading.Tasks;
using Redbean.MVP.Content;
using Redbean.Popup.Content;
using Redbean.Utility;
using UnityEngine;

namespace Redbean.Api
{
	public class GetAppConfigProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiGetRequest.GetAppConfigRequest(cancellationToken: cancellationToken);
			if (!response.isSuccess)
				return response;
			
			var app = this.GetModel<AppConfigModel>();
			app.Maintenance = response.Response.Maintenance;
			app.Version = response.Response.Version;
			app.Override();
			
#if UNITY_EDITOR || UNITY_ANDROID
			var version = app.Version.AndroidVersion;
#elif UNITY_EDITOR || UNITY_IOS
			var version = app.Version.iOSVersion;
#endif

			var isAppropriate = CompareVersion(version, Application.version);
			switch (isAppropriate)
			{
				// 정상 진입
				case <= 0:
				{
					// TODO : 정상 진입 로직

					LocalDatabase.Setup();
					LocalNotification.Setup();
					break;	
				}
						
				// 업데이트 필요
				case > 0:
				{
					// TODO : 업데이트 진입 로직

					AppLifeCycle.AppCheckFail();
					return response;
				}
			}

			if (!string.IsNullOrEmpty(app.Maintenance.Contents))
			{
				this.Popup().AssetOpen<PopupMaintenance>();
						
				AppLifeCycle.AppCheckFail();
				return response;
			}
					
			AppLifeCycle.AppCheckSuccess();
			return response;
		}
		
		private static int CompareVersion(string v1, string v2)
		{
			var v1a = v1.Split('.', StringSplitOptions.RemoveEmptyEntries);
			var v2a = v2.Split('.', StringSplitOptions.RemoveEmptyEntries);

			if (v1a.Length == 0 || v2a.Length == 0)
				return -1;
			
			var maxLength = v1a.Length > v2a.Length ? v1a.Length : v2a.Length;
			for (var i = 0; i < maxLength; i++)
			{
				int v1i = 0, v2i = 0;
				if (v1a.Length > i && !int.TryParse(v1a[i], out v1i)) 
					return -1;
				
				if (v2a.Length > i && !int.TryParse(v2a[i], out v2i))
					return -1;
				
				if (v1i != v2i) 
					return v1i.CompareTo(v2i);
			}

			return 0;
		}
	}
}