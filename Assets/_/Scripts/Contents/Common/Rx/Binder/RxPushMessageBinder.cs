using Firebase.Messaging;
using R3;

namespace Redbean.Rx
{
	public class RxPushMessageBinder : RxBase
	{
		private static readonly Subject<string> onPushTokenReceived = new();
		public static Observable<string> OnPushTokenReceived => onPushTokenReceived.Share();
		
		private static readonly Subject<FirebaseMessage> onPushMessageReceived = new();
		public static Observable<FirebaseMessage> OnPushMessageReceived => onPushMessageReceived.Share();

		protected override void Setup()
		{
			FirebaseMessaging.TokenReceived += OnTokenReceived;
			FirebaseMessaging.MessageReceived += OnMessageReceived;
		}

		protected override void Teardown()
		{
			FirebaseMessaging.TokenReceived -= OnTokenReceived;
			FirebaseMessaging.MessageReceived -= OnMessageReceived;
		}

		private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
		{
			onPushTokenReceived.OnNext(e.Token);
		}
		
		private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			onPushMessageReceived.OnNext(e.Message);
		}
	}
}