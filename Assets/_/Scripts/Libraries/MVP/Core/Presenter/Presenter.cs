using System;
using System.Linq;
using System.Reflection;
using R3;
using Redbean.Core;
using Redbean.Rx;
using UnityEngine;

namespace Redbean.MVP
{
	public class Presenter : IPresenter, IDisposable
	{
		private GameObject GameObject;

		public GameObject GetGameObject() => GameObject;

		public void BindView(IView view)
		{
			GameObject = view.GetGameObject();
				
			foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				var attributes = field.GetCustomAttributes(false);
				if (!attributes.Any())
					continue;

				foreach (var attribute in attributes)
					switch (attribute)
					{
						case ModelAttribute:
							Singleton.GetOrAdd<RxModelBinder>().OnModelChanged
							         .Where(_ => _.GetType() == field.FieldType)
							         .Subscribe(_ => field.SetValue(this, _))
							         .AddTo(this);
							
							field.SetValue(this, Model.GetOrAdd(field.FieldType));
							break;
						
						case ViewAttribute:
							field.SetValue(this, view);
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