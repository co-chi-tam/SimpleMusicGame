using System;
using UnityEngine;

namespace SimpleMusicGame {
	public class CNodeEditorData : CNodeData {

		public int editorIndex;
		public Vector2 editorNodePosition;

		public CNodeEditorData ()
		{
			this.editorIndex = 0;
			this.editorNodePosition = Vector2.zero;
		}
		
	}
}
