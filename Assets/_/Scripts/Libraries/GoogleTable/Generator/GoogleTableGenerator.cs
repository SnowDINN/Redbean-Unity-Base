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
		private static string SheetUri => DataContainer.Get<AppConfigModel>().Table.Uri;
		private static string SheetGid => DataContainer.Get<AppConfigModel>().Table.Gid;

		private static string Path =>
			$"{Application.dataPath.Replace("Assets", "")}{Resources.Load<GoogleTableInstaller>("GoogleTable/GoogleTable").Path}";

		private static string ItemPath =>
			$"{Application.dataPath.Replace("Assets", "")}{Resources.Load<GoogleTableInstaller>("GoogleTable/GoogleTable").ItemPath}";
		
		private const string Namespace = "Redbean";
		public int ExecutionOrder => 200;

		public async UniTask Setup() => await RuntimeTableSetup();
		public void Dispose() { }

		/// <summary>
		/// 테이블 데이터 호출 및 파싱
		/// </summary>
		public static async UniTask<Dictionary<string, string[]>> GetSheetRaw()
		{
			var csv = await GetCSV($"{SheetUri}/export?format=tsv&gid={SheetGid}");
			
			var idIndex = csv[0].Split("\t")
			                    .Select((key, index) => (key, index))
			                    .FirstOrDefault(_ => _.key == "Sheet ID");
			
			var nameIndex = csv[0].Split("\t")
			                      .Select((key, index) => (key, index))
			                      .FirstOrDefault(_ => _.key == "Sheet Name");

			var sheetRaw = new Dictionary<string, string[]>();
			for (var i = 1; i < csv.Length; i++)
			{
				var split = csv[i].Split("\t");
				var variables = await GetCSV($"{SheetUri}/export?format=tsv&gid={split[idIndex.index]}");
				var skipIndex = variables[0].Split("\t")
				                            .Select((key, index) => (key, index))
				                            .Where(_ => _.key.Contains('~'))
				                            .ToArray();

				for (var index = 0; index < variables.Length; index++)
				{
					var sheetValues = variables[index].Split("\t").ToList();
					var removeTarget = skipIndex.Select(index => sheetValues[index.index]).ToList();

					foreach (var target in removeTarget)
						sheetValues.Remove(target);

					variables[index] = string.Join("\t", sheetValues);
				}
				
				sheetRaw.Add(split[nameIndex.index], variables);
			}

			return sheetRaw;
		}
		
		/// <summary>
		/// 테이블 C# 스크립트 생성
		/// </summary>
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
				stringBuilder.AppendLine($"\t\tpublic static Dictionary<{table.Value[1].Split("\t").First()}, {table.Key}> {table.Key} = new();");
			
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");
			
			if (Directory.Exists(Path))
				Directory.CreateDirectory(Path);
			
			File.Delete($"{Path}/GoogleTable.cs");
			await File.WriteAllTextAsync($"{Path}/GoogleTable.cs", $"{stringBuilder}");
		}

		/// <summary>
		/// 테이블 아이템 C# 스크립트 생성
		/// </summary>
		public static async UniTask GenerateItemCSharp(string key, string[] value)
		{
			var variableNames = value[0].Split("\t");
			var variableTypes = value[1].Split("\t");

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"namespace {Namespace}.Table");
			stringBuilder.AppendLine("{");
			stringBuilder.AppendLine($"\tpublic class {key} : {nameof(IGoogleTable)}");
			stringBuilder.AppendLine("\t{");
			
			for (var i = 0; i < variableNames.Length; i++)
				stringBuilder.AppendLine($"\t\tpublic {variableTypes[i]} {variableNames[i]};");
			
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"\t\tpublic void {nameof(IGoogleTable.Apply)}(string value)");
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
					"int[]" => $"Array.ConvertAll(split[{i}].Split('|'), int.Parse),",
					"long[]" => $"Array.ConvertAll(split[{i}].Split('|'), long.Parse),",
					"float[]" => $"Array.ConvertAll(split[{i}].Split('|'), float.Parse),",
					"double[]" => $"Array.ConvertAll(split[{i}].Split('|'), double.Parse),",
					"string[]" => $"split[{i}].Split('|'),",
					_ => $"split[{i}],"
				};

				stringBuilder.AppendLine($"\t\t\t\t{variableNames[i]} = {convert}");
			}
			
			stringBuilder.AppendLine("\t\t\t};");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"\t\t\tGoogleTable.{key}.Add(item.Id, item);");
			stringBuilder.AppendLine("\t\t}");
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");
			
			if (Directory.Exists(ItemPath))
				Directory.CreateDirectory(ItemPath);
			
			File.Delete($"{ItemPath}/{key}.cs");
			await File.WriteAllTextAsync($"{ItemPath}/{key}.cs", $"{stringBuilder}");
		}

		/// <summary>
		/// 테이블 적용
		/// </summary>
		public async UniTask RuntimeTableSetup()
		{
			var sheets = await GetSheetRaw();
			foreach (var sheet in sheets)
			{
				var type = Type.GetType($"{Namespace}.Table.{sheet.Key}");
				foreach (var item in sheet.Value.Skip(2))
				{
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to connect to the Google sheets.");
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