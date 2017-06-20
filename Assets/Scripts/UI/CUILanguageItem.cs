using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleGameMusic.UICustom {
	public class CUILanguageItem : MonoBehaviour {

		[SerializeField]	private Text m_LanguageText;
		[SerializeField]	private Image m_LanguageImage;
		[SerializeField]	private Button m_LanguageButton;

		public void SetUpItem(string text, Sprite image,  Action submit) {
			this.m_LanguageText.text = text;
			this.m_LanguageImage.sprite = image;
			this.m_LanguageButton.onClick.RemoveAllListeners ();
			this.m_LanguageButton.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}
			});
		}
		
	}
}
