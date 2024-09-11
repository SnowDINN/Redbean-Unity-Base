using System;
using UnityEngine;

namespace Redbean
{
	public class Indicator : IDisposable
	{
		public Indicator()
		{
			if (IndicatorMono.Indicator)
				IndicatorMono.Indicator.ActiveGameObject(true);
		}
		
		public void Dispose()
		{
			if (IndicatorMono.Indicator)
				IndicatorMono.Indicator.ActiveGameObject(false);
		}
	}
	
	public class IndicatorMono : MonoBehaviour
	{
		private static IndicatorMono indicator;
		public static IndicatorMono Indicator
		{
			get
			{
				if (indicator)
					return indicator;

				var resource = Resources.Load<GameObject>("Indicator");
				var go = Instantiate(resource);
				go.name = "[Indicator System]";
				
				DontDestroyOnLoad(go);
				
				indicator = go.GetComponent<IndicatorMono>();
				return indicator;
			}
		}
	}
}