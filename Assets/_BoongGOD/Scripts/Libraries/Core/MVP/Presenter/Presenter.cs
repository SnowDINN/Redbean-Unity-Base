using System;
using System.Linq;
using System.Reflection;

namespace Redbean.Static
{
	public class Presenter : IPresenter, IDisposable
	{
		public void BindView(IView view)
		{
			foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				var attributes = field.GetCustomAttributes(false);
				if (!attributes.Any())
					continue;

				foreach (var attribute in attributes)
					switch (attribute)
					{
						case ViewAttribute:
							field.SetValue(this, view);
							break;
						
						case ModelAttribute:
							field.SetValue(this, Model.GetOrAdd(field.FieldType));
							break;

						case SingletonAttribute:
							field.SetValue(this, Singleton.GetOrAdd(field.FieldType));
							break;
					}
			}
		}
		
		public virtual void Setup()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}