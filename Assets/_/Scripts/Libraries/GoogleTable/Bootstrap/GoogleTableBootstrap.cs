using System;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Redbean.Dependencies;
using Redbean.Firebase;
using Redbean.MVP.Content;

namespace Redbean.Table
{
	public class GoogleTableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 200;

		public async UniTask Setup()
		{
			var names = FirebaseBootstrap.Firestore.Collection(FirebaseDefine.Storage).Document(ApplicationSettings.Version);
			var snapshotAsync = await names.GetSnapshotAsync();
			if (!snapshotAsync.Exists)
				return;

			var tables = snapshotAsync.ConvertTo<StorageFileModel>().Table;
			foreach (var table in tables)
			{
				var storageReference = FirebaseBootstrap.Storage.GetReference(StoragePath.TableRequest(table));
				var bytes = await storageReference.GetBytesAsync(1000 * 1000);
				var tsv = Encoding.UTF8.GetString(bytes).Split("\r\n");
				
				// Skip Name and Type Rows
				var skipRows = tsv.Skip(2);
				foreach (var item in skipRows)
				{
					var type = Type.GetType($"{GoogleTableGenerator.Namespace}.Table.{table}");
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to connect to the Google sheets.");
		}
		
		public void Dispose() { }
	}
}