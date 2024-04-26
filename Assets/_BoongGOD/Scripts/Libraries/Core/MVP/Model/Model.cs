using System.Collections.Generic;

namespace Redbean.Static
{
	public class Model
	{
		private static readonly Dictionary<string, IModel> models = new();

		/// <summary>
		/// 모델 포함 여부
		/// </summary>
		public static bool isContains<T>() where T : IModel => models.ContainsKey(typeof(T).Name);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T Get<T>() where T : IModel => (T)models[typeof(T).Name];
		
		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model) where T : IModel
		{
			if (model is not IModel result)
				return default;

			models[result.GetType().Name] = result;
			return model;
		}
	}
}