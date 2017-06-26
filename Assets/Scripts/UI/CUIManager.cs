using System;
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

		[Header ("Game setting")]
		[SerializeField]	private Slider m_SoundVolumeSlider;

		private CRootTask m_Root;

		#endregion

		#region Implementation MonoBehavious

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Start ()
		{
			base.Start ();
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
		}

		#endregion

	}
}
