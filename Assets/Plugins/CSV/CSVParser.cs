
// 
// CSV文字列を解析
//

using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pul
{
    public class CSVParser
    {
		/// <summary>
		/// CSVをList<string[]>へ変換
		/// http://dobon.net/vb/dotnet/file/readcsvfile.html
		/// </summary>
        public static List<string[]> CsvToArrayList(string csvtext)
        {
            List<string[]> csvrecords = new List<string[]>();

            //前後の改行を削除しておく
            csvtext = csvtext.Trim(new char[] { '\r', '\n' });

            //一行取り出すための正規表現
            Regex regline = new Regex("^.*(?:\\n|$)", RegexOptions.Multiline);

            //1行のCSVから各フィールドを取得するための正規表現
            Regex regcsv = new Regex("\\s*(\"(?:[^\"]|\"\")*\"|[^,]*)\\s*,", RegexOptions.None);
            Match match_line = regline.Match(csvtext);

            while (match_line.Success) {

                //一行取り出す
                string line = match_line.Value;

                // 改行記号が"で囲まれているか調べる
                while ((CountString(line, "\"") % 2) == 1) {
                    match_line = match_line.NextMatch();
                    if (!match_line.Success) {
                        //Log.Error("CSV解析エラー\n{0}", csvtext);
                    }
                    line += match_line.Value;
                }

                // 行の最後の改行記号を削除
                line = line.TrimEnd(new char[] { '\r', '\n' });
                // 最後に「,」をつける
                line += ",";

                //1つの行からフィールドを取り出す
                List<string> fields = new List<string>();
                Match m = regcsv.Match(line);
                while (m.Success) {
                    string field = m.Groups[1].Value;
                    //前後の空白を削除
                    field = field.Trim();
                    //"で囲まれている時
                    if (field.StartsWith("\"") && field.EndsWith("\"")) {
                        //前後の"を取る
                        field = field.Substring(1, field.Length - 2);
                        //「""」を「"」にする
                        field = field.Replace("\"\"", "\"");
                    }
                    fields.Add(field);
                    m = m.NextMatch();
                }

                if (fields.Count > 0 && fields[0].Length > 0 && fields[0].Substring(0, 1) == "#") {
                    // コメント行の場合無視する
                    match_line = match_line.NextMatch();
                    continue;
                }

                fields.TrimExcess();
                csvrecords.Add(fields.ToArray());
                match_line = match_line.NextMatch();
            }

            csvrecords.TrimExcess();

            return csvrecords;
        }

        /// <summary>
        /// 指定された文字列内にある文字列が幾つあるか数える
        /// </summary>
        /// <param name="strInput">strFindが幾つあるか数える文字列</param>
        /// <param name="strFind">数える文字列</param>
        /// <returns>strInput内にstrFindが幾つあったか</returns>
        public static int CountString(string strInput, string strFind)
        {
            int count = 0;
            int pos = strInput.IndexOf(strFind);
            while (pos > -1) {
                count++;
                pos = strInput.IndexOf(strFind, pos + 1);
            }
            return count;
        }
    }
}


