using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbean.MVP.Content;

namespace Redbean.Table
{
	public class GoogleTableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 200;

		public async Task Setup()
		{
			var tables = this.GetModel<StorageFileModel>().Table;
			if (!tables.Any())
			{
				Log.Fail("Table", "Fail to load to the Google sheets.");
				return;
			}
			
			foreach (var table in tables)
			{
				var storageReference = Extension.Storage.GetReference(StoragePath.TableRequest(table));
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
				
				Log.Notice($"{table} Table load is complete.");
			}
			
			Log.Success("Table", "Success to load to the tables.");
		}
		
		public void Dispose() { }
	}
}