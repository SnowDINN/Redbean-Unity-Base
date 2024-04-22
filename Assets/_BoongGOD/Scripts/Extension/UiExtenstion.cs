using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine.UI;

public static class UiExtenstion
{
	public static Observable<Unit> AsButtonObservable(this Button button, int inputThrottle = 200)
	{
		return inputThrottle > 0 
			? button.onClick.AsObservable().Share().ThrottleFirst(TimeSpan.FromMilliseconds(inputThrottle)) 
			: button.onClick.AsObservable().Share();
	}

	public static async UniTask SmoothFill(this Image image, float start, float end, float duration, CancellationToken token = default)
	{
		image.fillAmount = start;
		
		if (token == default)
			await image.DOFillAmount(end, duration);
		else
			await image.DOFillAmount(end, duration).AwaitForComplete(cancellationToken: token);
	}
}
