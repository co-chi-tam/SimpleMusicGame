using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CNodeObject : CBaseBehavious, INodeObject {

		[SerializeField]	protected float m_Value = 0;

		protected bool m_Active = false;

		public virtual float GetValue() {
			return 1f - this.m_Value;
		}

		public virtual void SetValue(float value) {
			this.m_Value = value;
		}

		public virtual bool GetActive() {
			return this.m_Active;
		}

		public virtual void SetActive(bool value) {
			this.m_Active = value;
			this.gameObject.SetActive (value);
		}
		
	}
}
