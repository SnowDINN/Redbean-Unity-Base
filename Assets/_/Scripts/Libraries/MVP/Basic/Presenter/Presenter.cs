using System;
using System.Linq;
using System.Reflection;
using Redbean.Container;
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
							field.SetValue(this, ModelContainer.Get(field.FieldType));
							break;
						
						case ViewAttribute:
							field.SetValue(this, view);
							break;

						case SingletonAttribute:
							field.SetValue(this, SingletonContainer.Get(field.FieldType));
							break;
					}
			}
		}
		
		/// <summary>
		/// Presenter 생성 시 호출되는 함수
		/// </summary>
		public virtual void Setup() { }

		/// <summary>
		/// Presenter 파괴 시 호출되는 함수
		/// </summary>
		public virtual void Dispose() { }
	}
}