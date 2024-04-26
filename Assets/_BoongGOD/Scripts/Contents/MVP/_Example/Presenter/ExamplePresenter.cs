using R3;
using Redbean.Extension;
using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Content.MVP
{
	public class ExampleButtonPresenter : Presenter
	{
		[Singleton] private RxInputBinder rxInputBinder;
		[View] private ButtonView view;

		[Model] private AppConfigModel configModel;

		public static readonly ReactiveProperty<int> RxInteger = new();

		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				RxInteger.Value += 1;
				Log.Print($"{configModel.android.version}");
			}).AddTo(view);

			rxInputBinder.OnKeyInputDetected.Subscribe(_ =>
			{
				Log.Print($"{_}");
			}).AddTo(view);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {view.GetType().FullName}");
		}
	}
	
	public class ExampleTextPresenter : Presenter
	{
		[View] private TextView view;

		public override void Setup()
		{
			ExampleButtonPresenter.RxInteger.Subscribe(_ =>
			{
				view.Text.text = $"{_}";
			}).AddTo(view);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {view.GetType().FullName}");
		}
	}
}