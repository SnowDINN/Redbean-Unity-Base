using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		public static T AddModel<T>(this T model) where T : IModel => 
			model is not IModel ? default : Model.Add(model);

		public static T PublishModel<T>(this T model) where T : IModel => 
			model is not IModel ? default : Singleton.Get<RxModelBinder>().Publish(Model.Add(model));
	}
}