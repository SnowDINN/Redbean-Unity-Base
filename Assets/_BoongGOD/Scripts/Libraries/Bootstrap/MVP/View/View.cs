using System;
using System.Linq;
using Redbean.Base;
using UnityEngine;

namespace Redbean.Static
{
	public class View : MonoBase, IView
	{
		[Header("Presenter"), TargetPresenter]
		public string TargetPresenter;

		private IPresenter presenter;

		protected virtual void Awake()
		{
			var type = Type.GetType(TargetPresenter);
			presenter = AppDomain.CurrentDomain.GetAssemblies()
			                     .SelectMany(x => x.GetTypes())
			                     .Where(x => type != null
			                                 && type.IsAssignableFrom(x)
			                                 && typeof(IPresenter).IsAssignableFrom(x)
			                                 && !x.IsInterface
			                                 && !x.IsAbstract)
			                     .Select(x => (IPresenter)Activator.CreateInstance(Type.GetType(x.FullName)))
			                     .FirstOrDefault();
			
			presenter?.BindView(this);
			presenter?.Setup();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			presenter?.Teardown();
		}
	}
}