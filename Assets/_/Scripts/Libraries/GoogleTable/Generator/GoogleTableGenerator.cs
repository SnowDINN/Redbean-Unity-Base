using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Redbean.Dependencies;
using Redbean.MVP.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Redbean.Table
{
	public class GoogleTableGenerator : IApplicationBootstrap
	{
		private const string ConvertFormat = "export?format=tsv&gid=";
		private const string Namespace = "Redbean";

		private static string SheetUri => DataContainer.Get<AppConfigModel>().Table.Uri;
		private static string SheetGid => DataContainer.Get<AppConfigModel>().Table.Gid;
		
		public int ExecutionOrder => 200;

		public async UniTask Setup() => await RuntimeTableSetup();
		public void Dispose() { }

		public static async UniTask<Dictionary<string, string[]>> GetSheetRaw()
		{
			var csv = await GetCSV($"{SheetUri}/{ConvertFormat}{SheetGid}");
			
			var idIndex = csv[0].Split("\t")
			                    .Select((key, index) => (key, index))
			                    .FirstOrDefault(_ => _.key == "Sheet ID");
			
			var nameIndex = csv[0].Split("\t")
			                      .Select((key, index) => (key, index))
			                      .FirstOrDefault(_ => _.key == "Sheet Name");

			var tables = new Dictionary<string, string[]>();
			for (var i = 1; i < csv.Length; i++)
			{
				var split = csv[i].Split("\t");
				tables.Add(split[nameIndex.index], await GetCSV($"{SheetUri}/{ConvertFormat}{split[idIndex.index]}"));
			}

			return tables;
		}
		
		public static async UniTask GenerateCSharp(Dictionary<string, string[]> tables)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("using System.Collections.Generic;");
			stringBuilder.AppendLine($"using {Namespace}.Table;");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"namespace {Namespace}");
			stringBuilder.AppendLine("{");
			stringBuilder.AppendLine("\tpublic class GoogleTable");
			stringBuilder.AppendLine("\t{");

			foreach (var table in tables)
				stringBuilder.AppendLine($"\t\tpublic static Dictionary<{table.Value[1].Split("\t").First()}, {table.Key}> Item = new();");
			
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");

			var path = $"{Application.dataPath}/_/Scripts/Libraries/GoogleTable/Basic";
			if (Directory.Exists(path))
				Directory.CreateDirectory(path);
			
			File.Delete($"{path}/GoogleTable.cs");
			await File.WriteAllTextAsync($"{path}/GoogleTable.cs", $"{stringBuilder}");
		}

		public static async UniTask GenerateItemCSharp(string key, string[] value)
		{
			var variableNames = value[0].Split("\t");
			var variableTypes = value[1].Split("\t");

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"namespace {Namespace}.Table");
			stringBuilder.AppendLine("{");
			stringBuilder.AppendLine($"\tpublic class {key} : IGoogleTable");
			stringBuilder.AppendLine("\t{");
			
			for (var i = 0; i < variableNames.Length; i++)
				stringBuilder.AppendLine($"\t\tpublic {variableTypes[i]} {variableNames[i]};");
			
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("\t\tpublic void Injection(string value)");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine("\t\t\tvar split = value.Split(\"\\t\");");
			stringBuilder.AppendLine($"\t\t\tvar item = new {key}");
			stringBuilder.AppendLine("\t\t\t{");

			for (var i = 0; i < variableNames.Length; i++)
			{
				var type = variableTypes[i];
				var convert = type switch
				{
					"int" => $"int.Parse(split[{i}]),",
					"long" => $"long.Parse(split[{i}]),",
					"float" => $"float.Parse(split[{i}]),",
					"double" => $"double.Parse(split[{i}]),",
					"string" => $"split[{i}],",
					_ => ""
				};

				stringBuilder.AppendLine($"\t\t\t\t{variableNames[i]} = {convert}");
			}
			
			stringBuilder.AppendLine("\t\t\t};");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("\t\t\tGoogleTable.Item.Add(item.Id, item);");
			stringBuilder.AppendLine("\t\t}");
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");

			var path = $"{Application.dataPath}/_/Scripts/Libraries/GoogleTable/Table";
			if (Directory.Exists(path))
				Directory.CreateDirectory(path);
			
			File.Delete($"{path}/{key}.cs");
			await File.WriteAllTextAsync($"{path}/{key}.cs", $"{stringBuilder}");
		}

		public async UniTask RuntimeTableSetup()
		{
			var sheets = await GetSheetRaw();
			foreach (var sheet in sheets)
			{
				var type = Type.GetType($"{Namespace}.Table.{sheet.Key}");
				foreach (var item in sheet.Value.Skip(2))
				{
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Injection(item);
				}
			}
		}

		private static async UniTask<string[]> GetCSV(string uri)
		{
			var www = UnityWebRequest.Get(uri);
			var request = await www.SendWebRequest();
			var csv = request.downloadHandler.text;
			
			return csv.Split("\r\n");
		}
	}
}