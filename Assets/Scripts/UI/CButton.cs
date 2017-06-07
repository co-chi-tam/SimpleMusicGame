using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UICustom {
	[RequireComponent (typeof (Image))]
	public class CButton : Button {

		public UnityEvent OnPress;
		public UnityEvent OnHold;
		public UnityEvent OnLeave;

		private Button m_Button;
		private float m_PressingTime = 0f;
		private bool m_IsPress = false;

		protected virtual void Awake() {
			this.m_Button = this.GetComponent<Button> ();
		}

		protected virtual void LateUpdate() {
			if (this.m_IsPress) {
				this.m_PressingTime += Time.deltaTime;
				if (this.m_PressingTime >= 0.25f) {
					this.OnHold.Invoke ();
				}
			}
		}

		public override void OnPointerDown (PointerEventData eventData)
		{
			base.OnPointerDown (eventData);
			this.m_PressingTime = 0f;
			this.m_IsPress = true;
			this.OnPress.Invoke ();
		}

		public override void OnPointerUp (PointerEventData eventData)
		{
			base.OnPointerUp (eventData);
			this.m_PressingTime = 0f;
			this.m_IsPress = false;
			this.OnLeave.Invoke ();
		}
		
	}
}
