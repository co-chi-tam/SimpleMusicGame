using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMusicGame.UICustom {
	public class CUISongItem : MonoBehaviour {

		public Text songNameText;
		public Image songBGImage;
		public Image songHardPoint;
		public Button songSubmit;

		public void SetUpSongItem(string songName, Sprite songBG, int hard, Action submit) {
			this.songNameText.text = songName;
			this.songBGImage.sprite = songBG;
			this.songHardPoint.fillAmount = (float)hard / 5f;
			this.songSubmit.onClick.RemoveAllListeners ();
			this.songSubmit.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}
			});
		}
		
	}
}
