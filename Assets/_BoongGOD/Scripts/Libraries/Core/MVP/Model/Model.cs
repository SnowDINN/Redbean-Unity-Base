using System;
using System.Collections.Generic;

namespace Redbean.Static
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
		public static T GetOrAdd<T>() where T : IModel
		{
			if (models.TryGetValue(typeof(T), out var value))
				return (T)value;

			models[typeof(T)] = (IModel)Activator.CreateInstance(typeof(T));
			return (T)models[typeof(T)];
		}

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static IModel GetOrAdd(Type type)
		{
			if (models.TryGetValue(type, out var value))
				return value;

			models[type] = (IModel)Activator.CreateInstance(type);
			return models[type];
		}

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model) where T : IModel
		{
			if (model is not IModel result)
				return default;

			models[result.GetType()] = result;
			return model;
		}
	}
}