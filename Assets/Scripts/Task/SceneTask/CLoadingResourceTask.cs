using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Pul;

namespace SimpleMusicGame {
	public class CLoadingResourceTask : CSimpleTask {

		#region Properties

		private CRequest m_Request;
		private CDownloadResourceManager m_ResourceManager;
		private CUILoading m_UILoading;

		private long m_CurrentTime;
		private bool m_OnLoadingProcess;

		#endregion

		#region Constructor

		public CLoadingResourceTask () : base ()
		{
			this.taskName = "LoadingResource";
			var firstSetting = PlayerPrefs.GetInt (CTaskUtil.GAME_FIRST_LAUNCH, 0) == 1;
			if (firstSetting == false) {
				this.nextTask = "LocalSetting";
			} else {
				this.nextTask = "SelectSong";
			}
#if UNITY_EDITOR || UNITY_STANDALONE
			this.m_Request = new CRequest (CTaskUtil.HOST + "/version?plf=standalone");
#else
			this.m_Request = new CRequest (CTaskUtil.HOST + "/version?plf=android");
#endif
			this.m_CurrentTime = DateTime.Now.Ticks;
			this.m_OnLoadingProcess = false;
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UILoading = CUILoading.GetInstance ();
			this.LoadVerionInfo ();
		}

		public override void OnTaskCompleted ()
		{
			base.OnTaskCompleted ();
			this.LoadLanguageCode();
			this.LoadSongList();
			this.LoadSetting ();
			this.m_OnLoadingProcess = false;
		}

		public override void OnTaskFail ()
		{
			base.OnTaskFail ();
			this.m_UILoading.ShowRetryButton ("RETRY", this.LoadVerionInfo);
		}

		#endregion

		#region Main methods

		public void LoadVerionInfo() {
			if (this.m_OnLoadingProcess == true)
				return;
			this.m_OnLoadingProcess = true;
			this.m_Request.Get ((result) => {
				var json 			= result.ToJSONObject();
				var version 		= int.Parse (json["version"].ToString());
				var subVersion		= json["subVersion"].ToString();
				var assetBundleUrl 	= json["assetBundleUrl"].ToString();
				this.m_CurrentTime 	= DateTime.UtcNow.Ticks; // long.Parse (json["serverTime"].ToString());
				this.m_ResourceManager = new CDownloadResourceManager (version, subVersion, assetBundleUrl);
				// COMPLETE
				this.DownloadResource();
				// UPDATE REFERENCES
				CTaskUtil.REFERENCES[CTaskUtil.VERSION] = subVersion;
				this.m_OnLoadingProcess = false;
				this.m_UILoading.SetUpUILoading (subVersion);
			}, (error) => {
				CLog.LogError (error);
				// FAIL
				this.OnTaskFail();
				this.m_OnLoadingProcess = false;
			}, null);
		}

		public void DownloadResource() {
			this.m_ResourceManager.DownloadResource (() => {
				// COMPLETE
				this.OnTaskCompleted();
			}, (error) => {
				CLog.LogError (error);
				// FAIL
				this.OnTaskFail();
			}, (processing) => {
				this.m_UILoading.Processing (processing);
			});
		}

		public void LoadLocalResource() {
			this.m_ResourceManager.LoadLocalResource (() => {
				// COMPLETE
				this.OnTaskCompleted();
			}, (error) => {
				CLog.LogError (error);
				// FAIL
				this.OnTaskFail();
			}, (processing) => {
				this.m_UILoading.Processing (processing);
			});
		}

		private void LoadLanguageCode() {
			var laPath = CAssetBundleManager.LoadResourceOrBundle<TextAsset> ("la_Path");
			var listLanguages = CSVUtil.ToObject<CLanguageData> (laPath.text);
			var saveListLanguages = CTaskUtil.REFERENCES [CTaskUtil.LA_DISPLAY] as List<CLanguageData>;
			var distLanguage = CTaskUtil.REFERENCES [CTaskUtil.LA] as Dictionary<string, Dictionary<string, string>>;
			for (int i = 0; i < listLanguages.Count; i++) {
				var laData = listLanguages [i];
				var transPath = CAssetBundleManager.LoadResourceOrBundle<TextAsset> (laData.laFile);
				var transData = CSVUtil.ToObject<CTranslateData> (transPath.text);
				distLanguage [laData.laName] = new Dictionary<string, string> ();
				saveListLanguages.Add (laData);
				for (int x = 0; x < transData.Count; x++) {
					var data = transData [x];
					distLanguage [laData.laName] [data.transCode] = data.transDisplay;
				}
			}
		}

		private void LoadSongList() {
			var listSongTextAsset = CAssetBundleManager.LoadResourceOrBundle <TextAsset> ("List-song");
			var listSong = CSVUtil.ToObject<CSongData> (listSongTextAsset.text);
			var saveListSongs = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			for (int i = 0; i < listSong.Count; i++) {
				var data = listSong [i];
				saveListSongs.Add (data);
			}
		}

		private void LoadSetting() {
			// Language setting
			var laSetting = PlayerPrefs.GetString (CTaskUtil.LA_SETTING, "EN");
			CTaskUtil.Set (CTaskUtil.LA_SETTING, laSetting);
			// User energy display
			var currentEnergy = PlayerPrefs.GetInt (CTaskUtil.PLAYER_ENERGY, 10);
			var saveTimer =  long.Parse (PlayerPrefs.GetString (CTaskUtil.PLAYER_ENEGY_SAVE_TIMER, this.m_CurrentTime.ToString()));
			var firstTimer = long.Parse (PlayerPrefs.GetString (CTaskUtil.GAME_FIRST_TIME, this.m_CurrentTime.ToString()));
			var playerEnergy = CTaskUtil.Get (CTaskUtil.PLAYER_ENERGY) as CPlayerEnergy; 
			playerEnergy.currentEnergy 	= currentEnergy;
			playerEnergy.maxEnergy 		= 10;
			playerEnergy.incrementEnergy = 1;
			playerEnergy.currentTimer 	= this.m_CurrentTime;
			playerEnergy.saveTimer 		= saveTimer;
			playerEnergy.firstTimer 	= firstTimer;
			playerEnergy.StartCounting ();
			playerEnergy.CalculateEnergy ();
			// Song volume
			var soundVolume = PlayerPrefs.GetFloat (CTaskUtil.GAME_SOUND_VOLUME, 0.5f);
			CTaskUtil.Set (CTaskUtil.GAME_SOUND_VOLUME, soundVolume);
		}

		#endregion

	}
}
