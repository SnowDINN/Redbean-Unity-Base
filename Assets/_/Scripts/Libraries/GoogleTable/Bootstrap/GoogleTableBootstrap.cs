using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Redbean.Table
{
	public class GoogleTableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 200;

		public async UniTask Setup()
		{
			var sheets = new Dictionary<string, string[]>();
			foreach (var sheet in sheets)
			{
				var type = Type.GetType($"{GoogleTableGenerator.Namespace}.Table.{sheet.Key}");
				foreach (var item in sheet.Value.Skip(2))
				{
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to connect to the Google sheets.");
		}
		
		public void Dispose() { }
	}
}