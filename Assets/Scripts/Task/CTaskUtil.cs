using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public class CTaskUtil {

		public static string HOST 						= "HOST";
		public static string VERSION 					= "VERSION";
		public static string SELECTED_SONG 				= "SELECTED_SONG";
		public static string LIST_SONG 					= "LIST_SONG";
		public static string LA 						= "LANGUAGE";
		public static string LA_DISPLAY 				= "LANGUAGE_DISPLAY";
		public static string LA_SETTING 				= "LANGUAGE_SETTING";
		public static string GAME_FIRST_LAUNCH 			= "GAME_FIRST_LAUNCH";
		public static string GAME_FIRST_TIME 			= "GAME_FIRST_TIME";
		public static string GAME_RESOURCE_COMPLETED	= "GAME_RESOURCE_COMPLETED";
		public static string GAME_SOUND_VOLUME 			= "GAME_SOUND_VOLUME";
		public static string PLAYER_ENERGY 				= "PLAYER_ENERGY";
		public static string PLAYER_MAX_ENERGY			= "PLAYER_MAX_ENERGY";
		public static string PLAYER_ENEGY_SAVE_TIMER 	= "PLAYER_ENEGY_SAVE_TIMER";

		public static Dictionary<string, object> REFERENCES = new Dictionary<string, object> () { 
			{ HOST, 			"https://tamco-tinygame.rhcloud.com" },
			{ VERSION, 			"v.1.0.0" },
			{ SELECTED_SONG, 	new CSongData() },
			{ LIST_SONG, 		new List<CSongData>() },
			{ LA, 				new Dictionary<string, Dictionary<string, string>>() }, // [VN][GOOD] = [TOT]
			{ LA_DISPLAY, 		new List<CLanguageData>() },
			{ LA_SETTING, 		"EN" },
			{ GAME_FIRST_LAUNCH, false },
			{ GAME_FIRST_TIME, 	DateTime.UtcNow.Ticks.ToString() },
			{ GAME_RESOURCE_COMPLETED, false },
			{ GAME_SOUND_VOLUME, 1f },
			{ PLAYER_ENERGY, 	new CPlayerEnergy() },
			{ PLAYER_MAX_ENERGY, 10 },
			{ PLAYER_ENEGY_SAVE_TIMER, DateTime.UtcNow.Ticks }
		};

		public static string Translate(string code) {
			var la = REFERENCES[LA_SETTING].ToString();
			var transDist = REFERENCES [CTaskUtil.LA] as Dictionary<string, Dictionary<string, string>>;
			if (transDist.ContainsKey (la)) {
				return transDist [la] [code];
			}
			return string.Empty;
		}

		public static object Get(string name) {
			return REFERENCES [name];
		}

		public static T Get<T>(string name) {
			var value = REFERENCES [name];
			var convert = Convert.ChangeType (value, typeof(T));
			return (T)convert;
		}

		public static void Set(string name, object value) {
			REFERENCES [name] = value;
		}

	}

}
