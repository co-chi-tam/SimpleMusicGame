using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using UICustom;

namespace SimpleGameMusic {
	public class CUISelectGame : CMonoSingleton<CUISelectGame> {

		#region Properties

		[Header ("Category")]
		[SerializeField]	private GameObject m_GroupCategories;
		[SerializeField]	private CButton m_CategoryPrefabButton;

		[Header ("Song")]
		[SerializeField]	private GameObject m_GroupSongs;
		[SerializeField]	private CButton m_SongPrefabButton;

		[Header ("User info")]
		[SerializeField]	private Text m_EnergyDisplayText;
		[SerializeField]	private Text m_EnergyReloadTimeText;

		private CRootTask m_RootTask;
		private Dictionary<string, List<CSongData>> m_CategoryDictionary;

		#endregion

		#region Implementation MonoBehavious

		protected override void Awake ()
		{
			base.Awake ();
			this.m_CategoryDictionary = new Dictionary<string, List<CSongData>> ();
		}

		protected virtual void Start() {
			this.m_RootTask = CRootTask.GetInstance ();
		}

		#endregion

		#region Main methods

		public void SetEnergyDisplayText(string value) {
			this.m_EnergyDisplayText.text = value;
		}

		public void SetEnergyReloadText(string value) {
			this.m_EnergyReloadTimeText.text = value;
		}

		public void LoadCategories(List<CSongData> songs) {
			DestroyAllChild (m_GroupCategories);
			for (int i = 0; i < songs.Count; i++) {
				var songData = songs [i];
				var createCategory = false;
				if (m_CategoryDictionary.ContainsKey (songData.categoryName)) {
					createCategory = false;
				} else {
					m_CategoryDictionary [songData.categoryName] = new List<CSongData> ();
					createCategory = true;
				}
				m_CategoryDictionary [songData.categoryName].Add (songData);
				if (createCategory == false)
					continue;
				var optionButton = Instantiate<CButton> (m_CategoryPrefabButton);
				this.SetupButton (optionButton, this.m_GroupCategories, songData.categoryName, () => {
					SelectCategory(songData.categoryName);
				});
			}
			this.m_CategoryPrefabButton.gameObject.SetActive (false);
			this.LoadSongs (m_CategoryDictionary[m_CategoryDictionary.Keys.ToList()[0]]);
		}

		public void LoadSongs(List<CSongData> songs) {
			DestroyAllChild (m_GroupSongs);
			for (int i = 0; i < songs.Count; i++) {
				var songData = songs [i];
				var optionButton = Instantiate<CButton> (m_SongPrefabButton);
				this.SetupButton (optionButton, this.m_GroupSongs, songData.displaySongName, () => {
					SelectSong(songData.songName);
				});
			}
			this.m_SongPrefabButton.gameObject.SetActive (false);
		}

		public void SelectCategory(string name) {
			if (m_CategoryDictionary.ContainsKey (name)) {
				var listSongs = m_CategoryDictionary[name];
				LoadSongs (listSongs);
			} 
		}

		public void SelectSong(string name) {
			CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] = name;
			this.m_RootTask.GetCurrentTask ().OnTaskCompleted ();
		}

		private void SetupButton(CButton button, GameObject parent, string text, Action submit) {
			button.transform.SetParent (parent.transform);
			button.gameObject.SetActive (true);
			button.SetText (text);
			button.onClick.RemoveAllListeners ();
			button.onClick.AddListener (() => {
				if (submit != null) {
					submit();
				}
			});
		}

		private void DestroyAllChild(GameObject parent, int startIndex = 1) {
			var childCount = parent.transform.childCount;
			for (int i = startIndex; i < childCount; i++) {
				var child = parent.transform.GetChild (i);
				Destroy (child.gameObject);
			}
		}

		#endregion
		
	}
}
