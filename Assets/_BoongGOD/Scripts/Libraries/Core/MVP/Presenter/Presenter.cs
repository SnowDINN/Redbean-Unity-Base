using System;
using System.Linq;
using System.Reflection;

namespace Redbean.Static
{
	public class Presenter : IPresenter, IDisposable
	{
		protected object ViewProperty { get; private set; }
		
		public void BindView(IView view)
		{
			ViewProperty = view;
			
			foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				var attributes = field.GetCustomAttributes(false);
				if (attributes == null || attributes.Any() == false)
					continue;

				foreach (var attribute in attributes)
				{
					switch (attribute)
					{
						case ViewAttribute:
							field.SetValue(this, ViewProperty);
							break;

						case SingletonAttribute:
							field.SetValue(this, Singleton.Get(field.FieldType));
							break;
					}
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