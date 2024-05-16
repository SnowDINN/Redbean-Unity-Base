using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Redbean.Core;
using Redbean.MVP;

namespace Redbean.Dependencies
{
	public class DependenciesModel : IApplicationStarted
	{
		public int ExecutionOrder => 1;
		
		public UniTask Setup()
		{
			return UniTask.CompletedTask;
		}
		
		private static readonly Dictionary<Type, Model> models = new();
		
		/// <summary>
		/// 모델 전부 제거
		/// </summary>
		public static void Clear() => models.Clear();

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetOrAdd<T>() where T : Model
		{
			if (models.TryGetValue(typeof(T), out var value))
				return value as T;

			models[typeof(T)] = Activator.CreateInstance(typeof(T)) as Model;
			return models[typeof(T)] as T;
		}

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static Model GetOrAdd(Type type)
		{
			if (models.TryGetValue(type, out var value))
				return value;

			models[type] = Activator.CreateInstance(type) as Model;
			return models[type];
		}

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model) where T : Model
		{
			if (model is not Model result)
				return default;

			models[result.GetType()] = result;
			return model;
		}
	}
}