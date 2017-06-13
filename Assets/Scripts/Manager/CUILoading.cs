using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CUILoading : CMonoSingleton<CUILoading> {

		#region Properties

		[Header ("Processing")]
		[SerializeField]	private Image m_LoadingImage;

		#endregion

		#region Implementation MonoBehavious
	
		protected override void Awake ()
		{
			base.Awake ();
			this.m_LoadingImage.type = Image.Type.Filled;
			this.m_LoadingImage.fillMethod = Image.FillMethod.Horizontal;
			this.m_LoadingImage.fillOrigin = 0;
			this.m_LoadingImage.fillAmount = 0;
		}

		#endregion

		#region Main methods

		public void Processing(float value) {
			this.m_LoadingImage.fillAmount = value;
		}

		#endregion

	}
}
