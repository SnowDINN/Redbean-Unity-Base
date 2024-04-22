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

	private const string FillToken = "TOKEN_FILL_AMOUNT";
	private const string IndexData = "INDEXS";
	private string index;
	
	private void Start()
	{
		index = this.GetSingleton<RxDataBinder>().Get<string>(IndexData);
		
		button.AsButtonObservable().Subscribe(_ =>
		{
			index += "1";
			index.Save(IndexData);
			UniTask.Void(ActiveFill, GenerateCancellationToken(FillToken).Token);
		}).AddTo(this);

		this.GetSingleton<RxDataBinder>().OnDataChanged.Subscribe(_ =>
		{
			Debug.Log(_.value);
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
