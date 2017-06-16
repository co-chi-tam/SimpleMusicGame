using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UICustom {
	public class CSlider : Slider {

		[Header("Event")]
		public UnityEvent OnSlideEnd;	

		private float m_RemainValue;
		private bool m_Ended;

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

	}
}
