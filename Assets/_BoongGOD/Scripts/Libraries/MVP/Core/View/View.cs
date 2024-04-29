using System;
using System.Linq;
using Redbean.Base;
using UnityEngine;

namespace Redbean.MVP
{
	public class View : MonoBase, IView
	{
		[Header("Presenter"), Presenter]
		public string PresenterFullName;
		
		private Presenter presenter;
		
		public virtual void Awake()
		{
			var type = Type.GetType(PresenterFullName);
			presenter = AppDomain.CurrentDomain.GetAssemblies()
			                     .SelectMany(x => x.GetTypes())
			                     .Where(x => type != null
			                                 && type.IsAssignableFrom(x)
			                                 && typeof(Presenter).IsAssignableFrom(x)
			                                 && !x.IsInterface
			                                 && !x.IsAbstract)
			                     .Select(x => (Presenter)Activator.CreateInstance(Type.GetType(x.FullName)))
			                     .FirstOrDefault();
			
			presenter?.BindView(this);
			presenter?.Setup();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			
			presenter?.Dispose();
		}
		
		public GameObject GetGameObject() => gameObject;
	}
}