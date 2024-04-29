using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.MVP;
using Redbean.Rx;

namespace Redbean.Content.MVP
{
	public class ExampleButtonPresenter : Presenter
	{
		[Singleton] 
		private RxInputBinder rxInputBinder;
		
		[View]
		private ButtonView view;

		[Model(SubscribeType.Subscribe)]
		private AppConfigModel configModel;

		public static readonly ReactiveProperty<int> RxInteger = new();

		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				RxInteger.Value += 1;
				Log.Print($"{configModel.android.version}");
			}).AddTo(this);

			rxInputBinder.OnKeyInputDetected.Subscribe(_ =>
			{
				Log.Print($"{_}");
			}).AddTo(this);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {view.GetType().FullName}");
		}
	}
	
	public class ExampleTextPresenter : Presenter
	{
		[View]
		private TextView view;

		public override void Setup()
		{
			ExampleButtonPresenter.RxInteger.Subscribe(_ =>
			{
				view.Text.text = $"{_}";
			}).AddTo(this);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {view.GetType().FullName}");
		}
	}
}