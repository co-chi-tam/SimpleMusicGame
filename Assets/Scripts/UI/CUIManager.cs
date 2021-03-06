﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleMusicGame.UICustom;

namespace SimpleMusicGame {
	public class CUIManager : CMonoSingleton<CUIManager> {

		#region Properties

		[Header("Root node")]
		[SerializeField]	private GameObject m_RootNode;

		[Header("Player info")]
		[SerializeField]	private Text m_ScoreText;
		[SerializeField]	private Animator m_ScoreAnimator;

		[Header ("Game setting")]
		[SerializeField]	private Slider m_SoundVolumeSlider;

		private CRootTask m_Root;

		#endregion

		#region Implementation MonoBehavious

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected virtual void Start ()
		{
			this.m_Root = CRootTask.GetInstance ();
		}

		#endregion

		#region Main methods

		public void SetSoundVolume(float value) {
			this.m_SoundVolumeSlider.value = value;
		}

		public void ChangeVolume(float value) {
			CTaskUtil.Set (CTaskUtil.GAME_SOUND_VOLUME, value);
			PlayerPrefs.SetFloat (CTaskUtil.GAME_SOUND_VOLUME, value);
			PlayerPrefs.Save ();
		}

		public void PreviousTask() {
			this.m_Root.PreviousTask ();
		}

		public void SetPlayerScore(string value) {
			this.m_ScoreText.text = "Score: " + value;
			CHandleEvent.Instance.AddEvent (
				this.HandleSetPlayerScore(this.m_ScoreText.fontSize, this.m_ScoreText.fontSize + 11), null);
		}

		private IEnumerator HandleSetPlayerScore(int sizeTo, int sizeFrom) {
			for (int i = sizeTo; i < sizeFrom; i++) {
				this.m_ScoreText.fontSize = i;
				yield return WaitHelper.WaitFixedUpdate;
			}
			yield return null;
			for (int i = sizeFrom; i >= sizeTo; i--) {
				this.m_ScoreText.fontSize = i;
				yield return WaitHelper.WaitFixedUpdate;
			}
			this.m_ScoreText.fontSize = sizeTo;
		}

		#endregion

	}
}
