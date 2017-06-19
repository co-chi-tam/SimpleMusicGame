using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleGameMusic.UICustom;

namespace SimpleGameMusic {
	public class CSliderNode : CSimpleNode {

		#region Properties

		public UnityEvent OnSlideEnd;

		#endregion

		#region Implementation INode

		public override void OnSlideEndNode ()
		{
			base.OnSlideEndNode ();
			if (this.OnSlideEnd != null) {
				this.OnSlideEnd.Invoke ();
			}
		}

		public override float GetValue ()
		{
			var result = this.m_Complete ? 1f : 0f;
			for (int i = 0; i < this.m_NodeObjects.Length; i++) {
				if (this.m_NodeObjects [i] != this) {
					result += this.m_NodeObjects [i].GetValue ();
				}
			}
			return result;
		}

		#endregion
		
	}
}
