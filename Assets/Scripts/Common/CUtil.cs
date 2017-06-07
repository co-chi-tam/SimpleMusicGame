using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
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

	}
}
