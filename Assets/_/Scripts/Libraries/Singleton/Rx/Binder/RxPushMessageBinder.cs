using Firebase.Messaging;
using R3;
using Redbean.Base;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

namespace Redbean.Rx
{
	public class RxPushMessageBinder : RxBase
	{
		private readonly Subject<FirebaseMessage> onPushMessageReceived = new();
		public Observable<FirebaseMessage> OnPushMessageReceived => onPushMessageReceived.Share();

		private const string PERMISSION_ANDROID = "android.permission.POST_NOTIFICATIONS";
		
		public RxPushMessageBinder()
		{
			FirebaseMessaging.TokenReceived += OnTokenReceived;
			FirebaseMessaging.MessageReceived += OnMessageReceived;
		}
		
		public override void Dispose()
		{
			base.Dispose();
			
			FirebaseMessaging.TokenReceived -= OnTokenReceived;
			FirebaseMessaging.MessageReceived -= OnMessageReceived;
		}

		private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
		{

		}
		
		private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			onPushMessageReceived.OnNext(e.Message);
		}

		public void Setup()
		{
#if UNITY_ANDROID
			if (!Permission.HasUserAuthorizedPermission(PERMISSION_ANDROID))
				Permission.RequestUserPermission(PERMISSION_ANDROID);
			
			AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel
			{
				Id = Application.identifier,
				Name = Application.identifier,
				Importance = Importance.Default,
				Description = "Generic notifications",
			});
#elif UNITY_IOS
#endif
		}
		
		public void PushNotification(FirebaseMessage message)
		{
#if UNITY_ANDROID
			var notification = new AndroidNotification
			{
				Title = message.Notification.Title,
				Text = message.Notification.Body,
				FireTime = System.DateTime.Now
			};
			
			AndroidNotificationCenter.SendNotification(notification, Application.identifier);
#elif UNITY_IOS
#endif
		}
	}
}