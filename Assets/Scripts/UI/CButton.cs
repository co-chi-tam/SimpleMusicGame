using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UICustom {
	[RequireComponent (typeof (Image))]
	public class CButton : Button {

		[Header("Info")]
		[SerializeField]	private Text m_ButtonDisplayText;

		private Button m_Button;
		private float m_PressingTime = 0f;
		private bool m_IsPress = false;

		[Header("Event")]
		public UnityEvent OnPress;
		public UnityEvent OnHold;
		public UnityEvent OnLeave;

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

		public virtual void SetText(string value) {
			if (this.m_ButtonDisplayText != null) {
				this.m_ButtonDisplayText.text = value;
			}
		}

		public virtual string GetText() {
			if (this.m_ButtonDisplayText != null) {
				return this.m_ButtonDisplayText.text;
			}
			return string.Empty;
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
