using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UICustom;

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
			base.GetValue ();
			if (this.m_Complete)
				return 1f;
			return 0f;
		}

		#endregion
		
	}
}
