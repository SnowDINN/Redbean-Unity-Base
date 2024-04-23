using System.Collections.Generic;

namespace Redbean.Static
{
	public class Model
	{
		public static readonly Dictionary<string, IModel> Models = new();

		public Model()
		{
			
		}

		~Model()
		{
			Models.Clear();
		}
		
		public static bool isContains<T>() => Models.ContainsKey(typeof(T).Name);
		
		public static T Get<T>() => (T)Models[typeof(T).Name];
		
		public static T Add<T>(T model)
		{
			if (model is not IModel result)
				return default;

			Models[result.GetType().Name] = result;
			return model;
		}
	}
}