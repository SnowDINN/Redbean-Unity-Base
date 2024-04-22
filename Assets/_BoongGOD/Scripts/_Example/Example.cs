using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBase
{
	[SerializeField] private Button button;
	[SerializeField] private Image image;
	
	private void Awake()
	{
		button.AsButtonObservable().Subscribe(_ =>
		{
			UniTask.Void(ActiveFill, CancellationTokenRefresh().Token);
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
