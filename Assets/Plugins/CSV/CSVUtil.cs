
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pul
{
	/// <summary>
	/// 単純なCSVの解析
	/// </summary>
	public static class CSVUtil
	{
		static CSVMapper mapper;

		static CSVUtil()
		{
			mapper = new CSVMapper();
			mapper.RegisterImporter<int>(new ImporterFunc<int>(StringToInt));
			mapper.RegisterImporter<long>(new ImporterFunc<long>(StringToLong));
			mapper.RegisterImporter<float>(new ImporterFunc<float>(StringToFloat));
			mapper.RegisterImporter<DateTime>(new ImporterFunc<DateTime>(StringToDateTime));
		}

		/// <summary>
		/// 基本的なCSVの解析
		/// </summary>
		public static List<T> ToObject<T>(string text)
			where T : new()
		{
			List<T> data = new List<T>();

			try {
				data = mapper.ToObject<T>(text);
			} catch (Exception e) {
				// 解析失敗
				//Log.Error("ToObject Failure e:{0} text:{1}", e, text);
			}
			return data;
		}

		// 文字列をint型へ変換
		private static int StringToInt(string str)
		{
			// "NULL"は0に変換
			if (str.ToLower() == "null") {
				return 0;
			}
			return System.Convert.ToInt32(str);
		}

		// 文字列をlong型へ変換
		private static long StringToLong(string str)
		{
			// "NULL"は0に変換
			if (str.ToLower() == "null") {
				return 0;
			}
			return System.Convert.ToInt64(str);
		}

		// 文字列をfloat型へ変換
		private static float StringToFloat(string str)
		{
			// "NULL"は0に変換
			if (str.ToLower() == "null") {
				return 0;
			}
			return System.Convert.ToSingle(str);
		}

		// 文字列を日付に変換する
		private static DateTime StringToDateTime(string str)
		{
			DateTime date = new DateTime();
			try {
				date = DateTime.ParseExact(str, "yyyy-MM-dd HH:mm:ss", null);
			} catch (Exception e) {
				//Log.Error("convertStringToDateTime str:{0} Failure e:{1}", str, e);
			}
			return date;
		}
	}
}

