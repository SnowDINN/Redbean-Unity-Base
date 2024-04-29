using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// UI 버튼 Observable 전환
		/// </summary>
		public static Observable<Unit> AsButtonObservable(this Button button, int inputThrottle = Balance.DoubleButtonPrevention) =>
			inputThrottle > 0 
				? button.onClick.AsObservable().Share().ThrottleFirst(TimeSpan.FromMilliseconds(inputThrottle)) 
				: button.onClick.AsObservable().Share();

		/// <summary>
		/// Image의 Fill Amount 애니메이션
		/// </summary>
		public static async UniTask SmoothFill(this Image image, float start, float end, float duration, CancellationToken token = default)
		{
			image.fillAmount = start;
		
			if (token == default)
				await image.DOFillAmount(end, duration);
			else
				await image.DOFillAmount(end, duration).AwaitForComplete(cancellationToken: token);
		}

		public static void ActiveCanvas(this CanvasGroup canvasGroup, bool value)
		{
			canvasGroup.alpha = value ? 1.0f : 0.0f;
			canvasGroup.interactable = value;
			canvasGroup.blocksRaycasts = value;
		}
	}	
}