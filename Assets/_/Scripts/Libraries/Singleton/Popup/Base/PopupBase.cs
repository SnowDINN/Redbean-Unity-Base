using Cysharp.Threading.Tasks;
using Redbean.Base;
using Redbean.Dependencies;

namespace Redbean.Popup
{
	public class PopupBase : MonoBase
	{
		private PopupManager Popup;

		public virtual void Awake() => Popup = DependenciesSingleton.GetOrAdd<PopupManager>();

		public virtual void Close() => Popup.Close(GetType());

		public void Destroy() => Destroy(gameObject);

		public async UniTask WaitUntilClose() => await DestroyCancellation.Token.WaitUntilCanceled();
	}
}