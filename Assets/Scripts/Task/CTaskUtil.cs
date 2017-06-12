using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CTaskUtil {

		public static string HOST 				= "http://tamco-tinygame.rhcloud.com";
		public static string VERSION 			= "VERSION";
		public static string SELECTED_SONG 		= "SELECTED_SONG";
		public static string DATA_SONG 			= "DATA_SONG";
		public static string LIST_SONG 			= "LIST_SONG";
		public static string LA 				= "LANGUAGE";
		public static string LA_DISPLAY 		= "LANGUAGE_DISPLAY";
		public static string LA_SETTING 		= "LANGUAGE_SETTING";
		public static string GAME_FIRST_LAUNCH 	= "GAME_FIRST_LAUNCH";

		public static Dictionary<string, object> REFERENCES = new Dictionary<string, object> () { 
			{ HOST, "http://tamco-tinygame.rhcloud.com" },
			{ VERSION, 1 },
			{ SELECTED_SONG, "Yeu-5" },
			{ DATA_SONG, new CSongData() },
			{ LIST_SONG, new List<CSongData>() },
			{ LA, new Dictionary<string, Dictionary<string, string>>() }, // [VN][GOOD] = [TOT]
			{ LA_DISPLAY, new List<CLanguageData>() },
			{ LA_SETTING, "EN" },
			{ GAME_FIRST_LAUNCH, false }
		};

		public static string Translate(string code) {
			var la = REFERENCES[LA_SETTING].ToString();
			return (REFERENCES[CTaskUtil.LA] as Dictionary<string, Dictionary<string, string>>)[la][code];
		}

	}

}
