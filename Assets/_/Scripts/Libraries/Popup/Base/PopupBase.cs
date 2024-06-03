using System.Threading.Tasks;
using Redbean.Bundle;
using Redbean.Singleton;
using UnityEngine;

namespace Redbean.Popup
{
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public GameObjectBundle Bundle;
		
		[HideInInspector]
		public string Guid;

		public virtual void Close() => this.GetSingleton<PopupSingleton>().Close(Guid);

		public async Task WaitUntilClose() => await TaskExtension.WaitUntil(() => DestroyCancellation.Token.IsCancellationRequested);
		
		public void Destroy()
		{
			Destroy(gameObject);
			Bundle.Release();
		}
	}
}