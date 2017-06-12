using System;
using UnityEngine;

namespace SimpleGameMusic {
	public class CNodeData {

		public int audioTime;
		public int nodeType;
		public string nodePosition;
		public float nodeScale;

		public CNodeData ()
		{
			this.audioTime 		= 0;
			this.nodeType 		= 0;
			this.nodePosition 	= string.Empty;
			this.nodeScale 		= 0f;
		}

		public override string ToString ()
		{
			return string.Format ("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", 
				this.audioTime, 
				this.nodeType, 
				this.nodePosition, 
				this.nodeScale
			);
		}

	}
}
