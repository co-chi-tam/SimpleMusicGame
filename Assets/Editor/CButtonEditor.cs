﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace UICustom {
	[CustomEditor (typeof (CButton), true)]
	[CanEditMultipleObjects]
	public class CButtonEditor : SelectableEditor {

		public override void OnInspectorGUI ()
		{
//			base.OnInspectorGUI ();
			DrawDefaultInspector ();
		}
		
	}
}
