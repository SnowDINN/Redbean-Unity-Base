using System.Threading.Tasks;
using Redbean.Base;
using Redbean.Dependencies;
using UnityEngine;

namespace Redbean.Popup
{
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public string Guid;
		
		private PopupBinder Popup;

		public virtual void Awake() => Popup = SingletonContainer.Get<PopupBinder>();

		public virtual void Close() => Popup.Close(Guid);

		public void Destroy() => Destroy(gameObject);

		public async Task WaitUntilClose() => await TaskExtension.WaitUntil(() => DestroyCancellation.Token.IsCancellationRequested);
	}
}