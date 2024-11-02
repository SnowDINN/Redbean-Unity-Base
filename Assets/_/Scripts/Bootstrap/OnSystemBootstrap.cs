using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Popup.Content;
using Redbean.Rx;
using UnityEngine;

namespace Redbean
{
	public class OnSystemBootstrap : Bootstrap
	{
		private readonly CompositeDisposable disposables = new();

		private const int timeout = 3;
		
		protected override Task Setup()
		{
			Application.logMessageReceived += OnLogMessageReceived;
			
			AudioSystem.OnInitialize();
			IndicatorSystem.OnInitialize();
			InteractionSystem.OnInitialize();

			AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(RxBase).IsAssignableFrom(x)
				            && typeof(RxBase).FullName != x.FullName
				            && !x.IsInterface
				            && !x.IsAbstract)
				.ToList()
				.ForEach(_ => (Activator.CreateInstance(_) as RxBase).Start());

			RxApiBinder.OnRequest
				.Subscribe(_ =>
				{
					UniTask.Void(OnRequestTimeoutSequence, cancellationToken);
				}).AddTo(disposables);
			
			RxApiBinder.OnResponse
				.Subscribe(_ =>
				{
					OnResponseTimeoutSequence();
				}).AddTo(disposables);
			
			return Task.CompletedTask;
		}

		protected override Task Teardown()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			
			disposables.Clear();
			disposables.Dispose();

			return Task.CompletedTask;
		}

		private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			if (type != LogType.Exception)
				return;
			
			this.Popup().AssetOpen<PopupException>().ExceptionMessage = condition;
		}

		private async UniTaskVoid OnRequestTimeoutSequence(CancellationToken cancellationToken)
		{
			if (InteractionSystem.Interaction)
				InteractionSystem.Interaction.ActiveGameObject(false);

			await UniTask.Delay
				(TimeSpan.FromSeconds(timeout), cancellationToken: cancellationToken);

			if (!InteractionSystem.Interaction.isActiveAndEnabled)
			{
				if (IndicatorSystem.Indicator)
					IndicatorSystem.Indicator.ActiveGameObject(true);
			}
		}
		
		private void OnResponseTimeoutSequence()
		{
			if (InteractionSystem.Interaction)
				InteractionSystem.Interaction.ActiveGameObject(true);

			if (IndicatorSystem.Indicator)
				IndicatorSystem.Indicator.ActiveGameObject(false);
		}
	}
}