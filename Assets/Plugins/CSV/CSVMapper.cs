// CSVデータをオブジェクトに変換する。
//
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Pul
{
	public delegate Value ImporterFunc<Value>(string input);
	public delegate object InternalImporterFunc(string str);

	public class CSVMapper
	{
		List<string[]> csvRawData;
		string[] csvFieldName;
		Dictionary<Type, InternalImporterFunc> importerTable;

		public CSVMapper()
		{
			importerTable = new Dictionary<Type, InternalImporterFunc>();
			ignoreEmptyLine = false;
			UnRegisterImporters();
		}

		/// <summary>
		/// CSV文字列strをListへ変換する
		/// </summary>
		public List<T> ToObject<T>(string str)
			where T : new()
		{
			// //Log.Debug("str:\n---\n{0}\n---", str);
			// 渡された型のpublicなフィールドを取得
			// FieldInfo[] fields = typeof(T).GetFields();
			// 解析
			csvRawData = CSVParser.CsvToArrayList(str);
			//Log.Assert(csvRawData.Count > 0, "CsvHeaderError");

			// 最初の一行目をフィールド名として使用
			// var csvFieldName = csvRawData[0];
			// フィールド名が空白になっているものが見つかったらそれ以降を無視
			var names = new List<string>();
			foreach (var s in csvRawData[0]) {
				if (s == "")
					break;
				names.Add(s);
			}
			csvFieldName = names.ToArray();

			List<T> csvdata = new List<T>();

			for (int i = 1; i < csvRawData.Count; ++i) {

				string[] line = csvRawData[i];

				// 空白データしかない列は無視するとき
				if (ignoreEmptyLine) {
					bool empty_line = true;
					foreach (string value in line) {
						if (value != "") {
							empty_line = false;
							break;
						}
					}
					if (empty_line) {
						// //Log.Warning("空データしかない行がみつかりました。");
						continue;
					}
				}

				T field = new T();
				try {
					for (int j = 0; j < line.Length && j < csvFieldName.Count(); ++j) {
						// フィールド名に'#'が入っていたら無視
						if (csvFieldName[j].IndexOf('#') >= 0)
							continue;

						// フィールド名に対応したクラスのフィールドを取得
						FieldInfo fieldinfo = typeof(T).GetField(csvFieldName[j]);

						if (fieldinfo != null) {
							// 対応するフィールドがあるとき
							//Log.Assert(importerTable.ContainsKey(fieldinfo.FieldType), "{0}型へ変換できません。", fieldinfo.FieldType);

							// 文字列を適切な型に変換
							InternalImporterFunc importer = importerTable[fieldinfo.FieldType];
							fieldinfo.SetValue(field, importer(line[j]));

						} else if (fieldinfo == null && field as IDictionary<string, string> != null) {
							// ディクショナリに追加できるとき
							IDictionary<string, string> dic = field as IDictionary<string, string>;
							dic.Add(csvFieldName[j], line[j]);

						} else {
							// 追加できないとき
							//Log.Assert(fieldinfo != null, "'{0}' に '{1}'フィールドがありません。", typeof(T).FullName, csvFieldName[j]);
						}
					}
				} catch (Exception e) {
					//Log.Warning("CSVエラー line:{0} e:{1}", i, e.ToString());
					throw e;
				}

				// 追加
				csvdata.Add(field);
			}

			return csvdata;
		}


		/// <summary>
		/// インポート用関数登録
		/// </summary>
		public void RegisterImporter<Value>(ImporterFunc<Value> importer)
		{
			InternalImporterFunc importer_wrapper = delegate(string str) {
				return importer(str);
			};
			importerTable[typeof(Value)] = importer_wrapper;
		}

		/// <summary>
		/// インポート用関数解除
		/// </summary>
		public void UnRegisterImporters()
		{
			importerTable.Clear();
			RegisterImporter(new ImporterFunc<string>(StringToString));
		}

		/// <summary>
		/// 行がすべて空白だったら無視する
		/// </summary>
		public bool ignoreEmptyLine
		{
			get;
			set;
		}

		/// <summary>
		/// 文字列を文字列へ変換
		/// </summary>
		private string StringToString(string str)
		{
			return str; // そのまま
		}
	}
}


