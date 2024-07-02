using System.Threading.Tasks;
using R3;
using Redbean.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Popup
{
	public enum PopupType
	{
		Asset,
		Bundle
	}
	
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public PopupType Type;
		
		[HideInInspector]
		public int Guid;

		[Header("Close Buttons"), SerializeField]
		private Button[] buttons;

		public virtual void Awake()
		{
			foreach (var button in buttons)
				button.AsButtonObservable().Subscribe(_ => { Close(); }).AddTo(this);
		}

		public virtual void Close() => this.GetSingleton<PopupSingleton>().Close(Guid);

		public async Task WaitUntilClose() => await TaskExtension.WaitUntil(() => destroyCancellationToken.IsCancellationRequested);
	}
}