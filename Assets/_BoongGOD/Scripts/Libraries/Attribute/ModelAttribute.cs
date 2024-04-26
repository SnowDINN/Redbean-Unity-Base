using Redbean.Define;
using UnityEngine;

namespace Redbean.Static
{
	public class ModelAttribute : PropertyAttribute
	{
		public SubscribeType type;
		
		public ModelAttribute()
		{
		}
		
		public ModelAttribute(SubscribeType type)
		{
			this.type = type;
		}
	}
}