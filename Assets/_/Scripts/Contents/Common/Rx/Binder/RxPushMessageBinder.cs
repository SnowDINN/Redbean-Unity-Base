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

		public override void Setup()
		{
			base.Setup();
			
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
			onPushTokenReceived.OnNext(e.Token);
		}
		
		private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			onPushMessageReceived.OnNext(e.Message);
		}
	}
}