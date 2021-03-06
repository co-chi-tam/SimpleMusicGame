﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleMusicGame.UICustom;

namespace SimpleMusicGame {
	public class CUILoading : CMonoSingleton<CUILoading> {

		#region Properties

		[Header ("Processing")]
		[SerializeField]	private Image m_LoadingImage;

		[Header ("Control")]
		[SerializeField]	private CButton m_RetryButton;
		[SerializeField]	private Text m_VersionText;

		#endregion

		#region Implementation MonoBehavious
	
		protected override void Awake ()
		{
			base.Awake ();
			// Local load resource button
			this.m_RetryButton.gameObject.SetActive (false);
		}

		#endregion

		#region Main methods

		public void Processing(float value) {
			this.m_LoadingImage.fillAmount = value;
		}

		public void ShowRetryButton(string text, Action submit) {
			this.m_RetryButton.SetText (text);
			this.m_RetryButton.gameObject.SetActive (true);
			this.m_RetryButton.onClick.RemoveAllListeners ();
			this.m_RetryButton.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}	
			});
		}

		public void SetUpUILoading(string value) {
			// Processing image
			this.m_LoadingImage.type = Image.Type.Filled;
			this.m_LoadingImage.fillMethod = Image.FillMethod.Horizontal;
			this.m_LoadingImage.fillOrigin = 0;
			this.m_LoadingImage.fillAmount = 0;
			// Version
			this.m_VersionText.text = value;
		}

		#endregion

	}
}
