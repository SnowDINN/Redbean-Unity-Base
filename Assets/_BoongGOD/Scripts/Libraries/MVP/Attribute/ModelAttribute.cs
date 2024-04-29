using UnityEngine;

namespace Redbean.MVP
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