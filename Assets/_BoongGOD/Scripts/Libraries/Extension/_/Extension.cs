using Redbean.Static;
using UnityEngine;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		private static T GetSingleton<T>() => 
			Singleton.Get<T>();
	}

	public class Console
	{
		public static void Log(string tag, string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}] {message}</color>");
		}
		
		public static void Log(string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
		}
	}
}