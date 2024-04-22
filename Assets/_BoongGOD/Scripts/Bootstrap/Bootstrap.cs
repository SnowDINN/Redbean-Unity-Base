using Redbean.Base;
using UnityEngine;

namespace Redbean
{
	public class Bootstrap
	{
		private static Singleton Singleton;
		
		[RuntimeInitializeOnLoadMethod]
		public static void Setup()
		{
			Singleton = new Singleton();
		}
	}   
}
