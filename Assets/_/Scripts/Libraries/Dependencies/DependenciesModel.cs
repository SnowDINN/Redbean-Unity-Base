using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Redbean.Core;
using Redbean.MVP;

namespace Redbean.Dependencies
{
	public class DependenciesModel : IApplicationCore
	{
		private static readonly Dictionary<Type, IModel> models = new();
		
		public int ExecutionOrder => 1;
		
		public UniTask Setup()
		{
			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(x => x.GetTypes())
			                                .Where(x => typeof(IModel).IsAssignableFrom(x)
			                                            && !typeof(IRxModel).IsAssignableFrom(x)
			                                            && !x.IsInterface
			                                            && !x.IsAbstract)
			                                .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IModel);

			foreach (var singleton in nativeSingletons
				         .Where(model => models.TryAdd(model.GetType(), model)))
				Log.System($"Create instance {singleton.GetType().FullName}");
			
			return UniTask.CompletedTask;
		}

		public UniTask TearDown()
		{
			models.Clear();
			
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// 모델 전부 제거
		/// </summary>
		public static void Clear() => models.Clear();

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetOrAdd<T>() where T : IModel => (T)models[typeof(T)];

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static IModel GetOrAdd(Type type) => models[type];

		/// <summary>
		/// 모델 존재 여부
		/// </summary>
		public static bool IsContains<T>() where T : IModel => models.ContainsKey(typeof(T));
		
		/// <summary>
		/// 모델 존재 여부
		/// </summary>
		public static bool IsContains(Type type) => models.ContainsKey(type);

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model) where T : IModel
		{
			var targetFields = models[model.GetType()].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.CanWrite).ToArray();
			var copyFields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.CanWrite).ToArray();
			
			for (var i = 0; i < targetFields.Length; i++)
				targetFields[i].SetValue(models[model.GetType()], copyFields[i].GetValue(model));
			
			return model;
		}
	}
}