using System;
using System.Linq;
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
		
		protected override Task Setup()
		{
			Application.logMessageReceived += OnLogMessageReceived;

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
					if (InteractionMono.Interaction)
						InteractionMono.Interaction.ActiveGameObject(true);
				}).AddTo(disposables);
			
			RxApiBinder.OnResponse
				.Subscribe(_ =>
				{
					if (InteractionMono.Interaction)
						InteractionMono.Interaction.ActiveGameObject(false);
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

		private async UniTaskVoid TimeoutIndicatorAsync()
		{
			if (InteractionMono.Interaction)
				InteractionMono.Interaction.ActiveGameObject(true);

			await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
		}
	}
}