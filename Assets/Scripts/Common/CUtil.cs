using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public static class CUtil {

		public static Vector2 ToVector2(this string value) {
			var strSplit = value.Split ('|');
			var v2 = Vector2.zero;
			if (strSplit.Length == 2) {
				v2.x = float.Parse (strSplit [0]);
				v2.y = float.Parse (strSplit [1]);
			}
			return v2;
		}

		public static long ToTicks(this long value) {
			var ticks = ((value * 10000) + 621355968000000000);
			return ticks;
		}

		public static long ToTimer(this long value) {
			var timer = (value - 621355968000000000) / 10000;
			return timer;
		}

	}
}
