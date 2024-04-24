using R3;
using Redbean.Base;
using Redbean.Define;
using Redbean.Extension;
using UnityEngine;

namespace Redbean.Content.Presenter
{
	public class AppInitializePresenter : MonoBase
	{
		private void Awake()
		{
			Bootstrap.OnAppStarted.Where(_ => _ == AppStartedType.Success)
			         .Subscribe( _ =>
			         {
				         Console.Log("System", "Success App Initialized", Color.green);
			         }).AddTo(this);
		}
	}
}