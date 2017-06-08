using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CUILoading : CMonoSingleton<CUILoading> {

		[Header ("Processing")]
		[SerializeField]	private Image m_LoadingImage;
	
		protected override void Awake ()
		{
			base.Awake ();
			this.m_LoadingImage.type = Image.Type.Filled;
			this.m_LoadingImage.fillMethod = Image.FillMethod.Horizontal;
			this.m_LoadingImage.fillOrigin = 0;
			this.m_LoadingImage.fillAmount = 0;
		}

		public void Processing(float value) {
			this.m_LoadingImage.fillAmount = value;
		}


	}
}
