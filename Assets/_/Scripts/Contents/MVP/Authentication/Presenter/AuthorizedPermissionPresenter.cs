using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.Native;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class AuthorizedPermissionPresenter : Presenter
	{
		[View]
		private EmptyView view;

		[Singleton]
		private RxPushMessageBinder rxPushMessageBinder;
		
		public override void Setup()
		{
			rxPushMessageBinder.OnPushMessageReceived.Subscribe(_ =>
			{
				Log.Print($"On Message : {JsonConvert.SerializeObject(_)}");
			}).AddTo(this);
			
			UniTask.Void(SetupPermission);
		}

		public async UniTaskVoid SetupPermission()
		{
			await UniTask.WaitForEndOfFrame(view.DestroyCancellation.Token);
			
			await NativeNotification.Setup();
		}
	}
}