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

		[Header ("Control")]
		[SerializeField]	private Button m_LoadLocalResourceButton;

		#endregion

		#region Implementation MonoBehavious
	
		protected override void Awake ()
		{
			base.Awake ();
			// Processing image
			this.m_LoadingImage.type = Image.Type.Filled;
			this.m_LoadingImage.fillMethod = Image.FillMethod.Horizontal;
			this.m_LoadingImage.fillOrigin = 0;
			this.m_LoadingImage.fillAmount = 0;

			// Local load resource button
			this.m_LoadLocalResourceButton.gameObject.SetActive (false);
		}

		#endregion

		#region Main methods

		public void Processing(float value) {
			this.m_LoadingImage.fillAmount = value;
		}

		public void ShowLocalResourceLoading(Action submit) {
			this.m_LoadLocalResourceButton.gameObject.SetActive (true);
			this.m_LoadLocalResourceButton.onClick.RemoveAllListeners ();
			this.m_LoadLocalResourceButton.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}	
			});
		}

		#endregion

	}
}
