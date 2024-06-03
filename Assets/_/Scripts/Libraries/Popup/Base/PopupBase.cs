using System.Threading.Tasks;
using Redbean.Base;
using Redbean.Singleton;
using UnityEngine;

namespace Redbean.Popup
{
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public string Guid;
		
		private PopupSingleton Popup;

		public virtual void Awake() => Popup = this.GetSingleton<PopupSingleton>();

		public virtual void Close() => Popup.Close(Guid);

		public void Destroy() => Destroy(gameObject);

		public async Task WaitUntilClose() => await TaskExtension.WaitUntil(() => DestroyCancellation.Token.IsCancellationRequested);
	}
}