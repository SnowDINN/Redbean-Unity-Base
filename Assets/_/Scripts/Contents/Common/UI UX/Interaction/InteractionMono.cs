using System;
using UnityEngine;

namespace Redbean
{
	public class Interaction : IDisposable
	{
		public Interaction()
		{
			if (InteractionMono.Interaction)
				InteractionMono.Interaction.ActiveGameObject(true);
		}
		
		public void Dispose()
		{
			if (InteractionMono.Interaction)
				InteractionMono.Interaction.ActiveGameObject(false);
		}
	}
	
	public class InteractionMono : MonoBehaviour
	{
		private static InteractionMono interaction;
		public static InteractionMono Interaction
		{
			get
			{
				if (interaction)
					return interaction;

				var resource = Resources.Load<GameObject>("Interaction");
				var go = Instantiate(resource);
				go.name = "[Indicator System]";
				
				DontDestroyOnLoad(go);
				
				interaction = go.GetComponent<InteractionMono>();
				return interaction;
			}
		}
	}
}