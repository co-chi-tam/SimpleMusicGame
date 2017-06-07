﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UICustom;

namespace SimpleGameMusic {
	public class CHoldNode : CSimpleNode {

		public override void OnHoldNode() {
			base.OnHoldNode ();
			if (this.m_Value >= 1f) {
				this.OnEndHold.Invoke ();
			} 
		}

		public override float GetValue ()
		{
			if (this.m_Complete == false)
				return 0f;
			return this.m_Value - this.m_HoldingValue;
		}
		
	}
}
