using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SimpleMusicGame.UICustom {
	public class CSlider : Slider, INodeObject {

		[Header("Event")]
		public UnityEvent OnSlideEnd;	

		protected bool m_Active = false;
		protected float m_RemainValue;
		protected bool m_Ended;

		protected override void Awake ()
		{
			base.Awake ();
			this.m_RemainValue = this.value;
			this.m_Ended = false;
		}

		protected virtual void LateUpdate() {
			if (this.value == this.maxValue && this.m_Ended == false) {
				if (this.OnSlideEnd != null) {
					this.OnSlideEnd.Invoke();
				}
				this.m_Ended = true;
			}
			if (this.value < this.m_RemainValue) {
				this.value = this.m_RemainValue;
			} else {
				this.m_RemainValue = this.value;
			}
		}

		public virtual float GetValue() {
			return this.value;
		}

		public virtual void SetValue(float value) {
			this.value = value;
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
