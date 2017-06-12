using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CTaskUtil {

		public static string VERSION 		= "VERSION";
		public static string SELECTED_SONG 	= "SELECTED_SONG";
		public static string DATA_SONG 		= "DATA_SONG";
		public static string LA 			= "LANGUAGE";
		public static string LA_DISPLAY 	= "LANGUAGE_DISPLAY";

		public static Dictionary<string, object> REFERENCES = new Dictionary<string, object> () { 
			{ VERSION, 1 },
			{ SELECTED_SONG, "Yeu-5" },
			{ DATA_SONG, new CSongData() },
			{ LA, new Dictionary<string, Dictionary<string, string>>() }, // [VN][GOOD] = [TOT]
			{ LA_DISPLAY, new List<CLanguageData>() }
		};

		public static string Translate(string la, string code) {
			return (REFERENCES[CTaskUtil.LA] as Dictionary<string, Dictionary<string, string>>)[la][code];
		}

	}

}
