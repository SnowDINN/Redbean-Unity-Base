using System;
using System.Collections.Generic;

namespace Redbean.MVP
{
	public class Model
	{
		private static readonly Dictionary<Type, IModel> models = new();
		
		/// <summary>
		/// 모델 전부 제거
		/// </summary>
		public static void Clear() => models.Clear();

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetOrAdd<T>() where T : class, IModel
		{
			if (models.TryGetValue(typeof(T), out var value))
				return value as T;

			models[typeof(T)] = Activator.CreateInstance(typeof(T)) as IModel;
			return models[typeof(T)] as T;
		}

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static IModel GetOrAdd(Type type)
		{
			if (models.TryGetValue(type, out var value))
				return value;

			models[type] = Activator.CreateInstance(type) as IModel;
			return models[type];
		}

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model) where T : class, IModel
		{
			if (model is not IModel result)
				return default;

			models[result.GetType()] = result;
			return model;
		}
	}
}