using Firebase.Messaging;
using R3;

namespace Redbean.Rx
{
	public class RxPushMessageBinder : RxBase, ISingletonContainer
	{
		private readonly Subject<string> onPushTokenReceived = new();
		public Observable<string> OnPushTokenReceived => onPushTokenReceived.Share();
		
		private readonly Subject<FirebaseMessage> onPushMessageReceived = new();
		public Observable<FirebaseMessage> OnPushMessageReceived => onPushMessageReceived.Share();
		
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
			onPushTokenReceived.OnNext(e.Token);
		}
		
		private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			onPushMessageReceived.OnNext(e.Message);
		}
	}
}