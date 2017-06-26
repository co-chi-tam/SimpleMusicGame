using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMusicGame.UICustom {
	public class CUISongItem : MonoBehaviour {

		[Header ("Page info")]
		public int page;
		public CUIScrollRectSnapPage rootPage;
		public Animator animatorPage;
		[Header ("Song info")]
		public Text songText;
		public Text songNameText;
		public Image songBGImage;
		public Image songHardPoint;
		public Image playSongImage;
		public Button songSubmit;
		public Sprite playSprite;
		public Sprite adsSprite;

		private void Start() {
			this.rootPage.OnSnapPage -= OnPageActive;
			this.rootPage.OnSnapPage += OnPageActive;
		}

		private void OnPageActive(int index) {
			if (index == this.page) {
				this.animatorPage.SetTrigger ("Active");
			}
		}

		public void SetUpSongItem(int index, string songName, Sprite songBG, int hard, bool isAds, Action submit) {
			this.page = index;
			this.songText.text = CTaskUtil.Translate ("SONG");
			this.songNameText.text = songName;
			this.songBGImage.sprite = songBG;
			this.playSongImage.sprite = isAds ? adsSprite : playSprite;
			this.songHardPoint.fillAmount = (float)hard / 5f;
			this.songSubmit.onClick.RemoveAllListeners ();
			this.songSubmit.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}
			});
			this.OnPageActive (index);
		}
		
	}
}
