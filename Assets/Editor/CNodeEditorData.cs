using System;
using UnityEngine;

namespace SimpleGameMusic {
	public class CNodeEditorData : CNodeData {

		public int editorIndex;
		public Vector2 nodePositionEditor;

		public CNodeEditorData ()
		{
			this.editorIndex = 0;
			this.nodePositionEditor = Vector2.zero;
		}
		
	}
}
