using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using UICustom;

namespace SimpleGameMusic {
	public class CUISelectGame : CMonoSingleton<CUISelectGame> {

		[SerializeField]	private GameObject m_GroupButton;
		[SerializeField]	private CButton m_SongPrefabButton;

		private CRootTask m_RootTask;

		protected virtual void Start() {
			this.m_RootTask = CRootTask.GetInstance ();
		}

		public void LoadListSong(List<CSongData> songs) {
			for (int i = 0; i < songs.Count; i++) {
				var optionButton = Instantiate<CButton> (m_SongPrefabButton);
				var songData = songs [i];
				optionButton.transform.SetParent (this.m_GroupButton.transform);
				optionButton.gameObject.SetActive (true);
				optionButton.SetText (songData.displaySongName);
				optionButton.onClick.RemoveAllListeners ();
				optionButton.onClick.AddListener (() => {
					SelectGame(songData.songName);
				});
			}
			this.m_SongPrefabButton.gameObject.SetActive (false);
		}

		public void SelectGame(string name) {
			CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] = name;
			this.m_RootTask.GetCurrentTask ().OnTaskCompleted ();
		}
		
	}
}
