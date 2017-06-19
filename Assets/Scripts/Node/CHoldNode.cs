using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleGameMusic.UICustom;

namespace SimpleGameMusic {
	public class CHoldNode : CSimpleNode {

		#region Properties

		public UnityEvent OnStartHold;
		public UnityEvent OnEndHold;

		protected float m_HoldingValue = 0f;

		#endregion

		#region Implementation INode

		public override void OnPressNode ()
		{
			base.OnPressNode ();
			this.m_HoldingValue = this.m_Value;
		}

		public override void OnHoldNode() {
			base.OnHoldNode ();
			this.OnStartHold.Invoke ();
			if (this.m_Value >= 1f) {
				this.OnEndHold.Invoke ();
			} 
		}

		#endregion

		#region Getter && Setter

		public override float GetValue ()
		{
			if (this.m_Complete == false)
				return 0f;
			return this.m_Value - this.m_HoldingValue;
		}

		#endregion

	}
}
