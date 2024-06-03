using System.Threading.Tasks;
using Redbean.Singleton;
using UnityEngine;

namespace Redbean.Popup
{
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public int Guid;
		
		public virtual void Close() => this.GetSingleton<PopupSingleton>().Close(Guid);

		public async Task WaitUntilClose() => await TaskExtension.WaitUntil(() => DestroyCancellation.Token.IsCancellationRequested);
	}
}