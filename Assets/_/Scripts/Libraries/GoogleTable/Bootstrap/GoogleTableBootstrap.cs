using System;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Redbean.Dependencies;
using Redbean.Firebase;
using Redbean.MVP.Content;
using UnityEngine;

namespace Redbean.Table
{
	public class GoogleTableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 200;

		public async UniTask Setup()
		{
			var names = DataContainer.Get<TableConfigModel>().TableNames;
			foreach (var name in names)
			{
				var storageReference = FirebaseBootstrap.Storage.GetReference($"Table/{Application.version}/{name}.tsv");
				var bytes = await storageReference.GetBytesAsync(1000 * 1000);
				
				var tsv = Encoding.UTF8.GetString(bytes).Split("\r\n");
				var tsvRefined = GoogleTableGenerator.TsvRefined(tsv);
				foreach (var item in tsvRefined.Skip(2))
				{
					var type = Type.GetType($"{GoogleTableGenerator.Namespace}.Table.{name}");
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to connect to the Google sheets.");
		}
		
		public void Dispose() { }
	}
}