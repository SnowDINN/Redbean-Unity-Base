using Redbean.Static;
using UnityEngine;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		private static T GetSingleton<T>() where T : ISingleton => Singleton.GetOrAdd<T>();
	}

	public class Log
	{
		public static void Print(string tag, string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}] {message}</color>");
		}
		
		public static void Print(string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
		}
	}
}