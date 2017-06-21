using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleMusicGame.UICustom;

namespace SimpleMusicGame {
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
			var result = 0f;
			for (int i = 0; i < this.m_NodeObjects.Count; i++) {
				if (this.m_NodeObjects [i] != this) {
					result += this.m_NodeObjects [i].GetValue ();
				}
			}
			return result;
		}

		#endregion
		
	}
}
