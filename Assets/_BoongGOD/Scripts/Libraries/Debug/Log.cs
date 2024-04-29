using UnityEngine;

namespace Redbean.Debug
{
	public class Log
	{
		public static void Print(string tag, string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}] {message}</color>");
		}
		
		public static void Print(string message, Color color = default)
		{
			if (color == default)
				color = Color.white;
			
			UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
		}
	}
}