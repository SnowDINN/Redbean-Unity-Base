using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean.Static
{
	public class Model : IBootstrap
	{
		private static readonly Dictionary<string, IModel> Models = new();

		public Model()
		{
			var models = AppDomain.CurrentDomain.GetAssemblies()
			                      .SelectMany(x => x.GetTypes())
			                      .Where(x => typeof(IModel).IsAssignableFrom(x)
			                                  && !x.IsInterface
			                                  && !x.IsAbstract)
			                      .Select(x => (IModel)Activator.CreateInstance(Type.GetType(x.FullName)));
			
			foreach (var model in models
				         .Where(singleton => Models.TryAdd(singleton.GetType().Name, singleton)))
				Console.Log("Model", $" Create instance {model.GetType().FullName}", Color.cyan);
		}

		~Model()
		{
			Models.Clear();
		}
		
		/// <summary>
		/// 모델 포함 여부
		/// </summary>
		public static bool isContains<T>() => Models.ContainsKey(typeof(T).Name);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T Get<T>() => (T)Models[typeof(T).Name];
		
		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model)
		{
			if (model is not IModel result)
				return default;

			Models[result.GetType().Name] = result;
			return model;
		}
	}
}