using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Base;
using Redbean.Extension;
using Redbean.Rx;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBase
{
	[SerializeField] private Button button;
	[SerializeField] private Image image;

	private const string FillToken = "FillToken";
	
	private void Start()
	{
		button.AsButtonObservable().Subscribe(_ =>
		{
			UniTask.Void(ActiveFill, GenerateCancellationToken(FillToken).Token);
		}).AddTo(this);

		Singleton.Get<RxInputBinder>().KeyObservable.Subscribe(_ =>
		{
			Debug.Log(_);
		}).AddTo(this);

		Singleton.Get<RxInputBinder>().MouseAndTouchObservable.Subscribe(_ =>
		{
			Debug.Log(_);
		}).AddTo(this);
	}

	private async UniTaskVoid ActiveFill(CancellationToken token)
	{
		await image.SmoothFill(start: 0,
		                       end: 1,
		                       duration: 3,
		                       token: token);
		Debug.Log("!");
	}
}
