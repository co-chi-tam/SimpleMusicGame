using System;
using UnityEngine;

namespace SimpleGameMusic {
	public class CNodeEditorData : CNodeData {

		public int editorIndex;
		public int duplicateIndex;
		public Vector2 nodePositionEditor;

		public CNodeEditorData ()
		{
			this.editorIndex = 0;
			this.duplicateIndex = 1;
			this.nodePositionEditor = Vector2.zero;
		}
		
	}
}
