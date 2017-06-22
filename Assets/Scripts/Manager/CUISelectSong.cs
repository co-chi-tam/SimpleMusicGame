using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleMusicGame.UICustom;

namespace SimpleMusicGame {
	public class CUISelectSong : CMonoSingleton<CUISelectSong> {

		#region Properties

		[Header ("Category")]
		[SerializeField]	private GameObject m_GroupCategories;
		[SerializeField]	private CButton m_CategoryPrefabButton;

		[Header ("Song")]
		[SerializeField]	private GameObject m_GroupSongs;
		[SerializeField]	private CUISongItem m_SongPrefabButton;

		[Header ("User info")]
		[SerializeField]	private Text m_EnergyDisplayText;
		[SerializeField]	private Text m_EnergyReloadTimeText;

		[Header ("Game setting")]
		[SerializeField]	private Slider m_SoundVolumeSlider;

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
			var soundVolume = (float)CTaskUtil.Get (CTaskUtil.GAME_SOUND_VOLUME);
			this.m_SoundVolumeSlider.value = soundVolume;
		}

		#endregion

		#region Main methods

		public void SetEnergyDisplayText(string value) {
			this.m_EnergyDisplayText.text = value;
		}

		public void SetEnergyReloadText(string value) {
			this.m_EnergyReloadTimeText.text = value;
		}

		public void LoadListCategories(List<CSongData> songs) {
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
			this.LoadListSongs (m_CategoryDictionary[m_CategoryDictionary.Keys.ToList()[0]]);
		}

		public void LoadListSongs(List<CSongData> songs) {
			DestroyAllChild (m_GroupSongs);
			for (int i = 0; i < songs.Count; i++) {
				var songData = songs [i];
				var optionButton = Instantiate<CUISongItem> (m_SongPrefabButton);
				var bgSprite = CAssetBundleManager.LoadResourceOrBundle<Sprite> (songData.songName);
				this.SetupSongItem (optionButton, this.m_GroupSongs, songData.displaySongName, bgSprite, songData.hardPoint, () => {
					this.SelectSong(songData);
				});
			}
			this.m_SongPrefabButton.gameObject.SetActive (false);
		}

		public void SelectCategory(string name) {
			if (m_CategoryDictionary.ContainsKey (name)) {
				var listSongs = m_CategoryDictionary[name];
				LoadListSongs (listSongs);
			} 
		}

		public void SelectSong(CSongData song) {
			CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] = song;
			this.m_RootTask.GetCurrentTask ().OnTaskCompleted ();
		}

		private void SetupSongItem(CUISongItem item, GameObject parent, string text, Sprite bg, int hardPoint, Action submit) {
			var playerEnergy = CTaskUtil.Get (CTaskUtil.PLAYER_ENERGY) as CPlayerEnergy;
			item.transform.SetParent (parent.transform);
			item.gameObject.SetActive (true);
			item.SetUpSongItem (text, bg, hardPoint, playerEnergy.currentEnergy < hardPoint, submit);
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

		public void ChangeVolume(float value) {
			CTaskUtil.Set (CTaskUtil.GAME_SOUND_VOLUME, value);
			PlayerPrefs.SetFloat (CTaskUtil.GAME_SOUND_VOLUME, value);
			PlayerPrefs.Save ();
		}

		#endregion
		
	}
}
