using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxDeepLinkBinder : RxBase
	{
		private static readonly Subject<Dictionary<string, string>> onDeepLinkReceived = new();
		public static Observable<Dictionary<string, string>> OnDeepLinkReceived => onDeepLinkReceived.Share();

		public override void Setup()
		{
			base.Setup();
			
			Application.deepLinkActivated += OnDeepLinkActivated;
		}

		public override void Dispose()
		{
			base.Dispose();
			
			Application.deepLinkActivated -= OnDeepLinkActivated;
		}

		private void OnDeepLinkActivated(string uri)
		{
			var collection = new Dictionary<string, string>();
			
			var queryString = new Uri(uri).GetComponents(UriComponents.Query, UriFormat.SafeUnescaped);
			var queryCollection = queryString.Split('&')
				.Select(x => x.Split('='))
				.Where(x => x.Length == 2)
				.ToList();

			foreach (var query in queryCollection)
				collection.TryAdd(query[0], query[1]);
			
			onDeepLinkReceived.OnNext(collection);
		}
	}
}