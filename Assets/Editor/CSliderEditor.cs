using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace UICustom {
	[CustomEditor (typeof (CSlider), true)]
	[CanEditMultipleObjects]
	public class CCSliderEditor : SelectableEditor {

		public override void OnInspectorGUI ()
		{
//			base.OnInspectorGUI ();
			DrawDefaultInspector ();
		}

	}
}