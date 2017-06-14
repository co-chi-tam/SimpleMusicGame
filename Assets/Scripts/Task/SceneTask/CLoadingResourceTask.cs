﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Pul;

namespace SimpleGameMusic {
	public class CLoadingResourceTask : CSimpleTask {

		#region Properties

		private CRequest m_Request;
		private CDownloadResourceManager m_ResourceManager;
		private CUILoading m_UILoading;

		private long m_CurrentTime;

		#endregion

		#region Constructor

		public CLoadingResourceTask () : base ()
		{
			this.taskName = "LoadingResource";
			var firstSetting = PlayerPrefs.GetInt (CTaskUtil.GAME_FIRST_LAUNCH, 0) == 1;
			if (firstSetting == false) {
				this.nextTask = "LocalSetting";
			} else {
				this.nextTask = "SelectGame";
			}
#if UNITY_EDITOR
			this.m_Request = new CRequest (CTaskUtil.HOST + "/version?plf=standalone");
#else
			this.m_Request = new CRequest (CTaskUtil.HOST + "/version?plf=android");
#endif
			this.m_CurrentTime = DateTime.Now.Ticks;
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UILoading = CUILoading.GetInstance ();
			this.m_Request.Get ((result) => {
				var json 			= result.ToJSONObject();
				var version 		= int.Parse (json["version"].ToString());
				var versionString	= json["versionString"].ToString();
				var assetBundleUrl 	= json["assetBundleUrl"].ToString();
				this.m_CurrentTime 	= DateTime.UtcNow.Ticks; // long.Parse (json["serverTime"].ToString());
				this.m_ResourceManager = new CDownloadResourceManager (version, versionString, assetBundleUrl);
				// COMPLETE
				this.DownloadResource();
				// UPDATE REFERENCES
				CTaskUtil.REFERENCES[CTaskUtil.VERSION] = version;
			}, (error) => {
				CLog.LogError (error);
				// FAIL
				this.OnTaskFail();
			}, null);
		}

		public override void OnTaskCompleted ()
		{
			base.OnTaskCompleted ();
			this.LoadLanguageCode();
			this.LoadSongList();
			this.LoadSetting ();
		}

		public override void OnTaskFail ()
		{
			base.OnTaskFail ();
			this.m_UILoading.ShowLocalResourceLoading (this.LoadLocalResource);
		}

		#endregion

		#region Main methods

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
			CTaskUtil.REFERENCES [CTaskUtil.LA_SETTING] = laSetting;
			// User energy display
			var currentEnergy = PlayerPrefs.GetInt (CTaskUtil.PLAYER_ENERGY, 10);
			var saveTimer = long.Parse (PlayerPrefs.GetString (CTaskUtil.PLAYER_ENEGY_SAVE_TIMER, this.m_CurrentTime.ToString()));
			var playerEnergy = CTaskUtil.REFERENCES [CTaskUtil.PLAYER_ENERGY] as CPlayerEnergy; 
			playerEnergy.currentEnergy 	= currentEnergy;
			playerEnergy.maxEnergy 		= 10;
			playerEnergy.currentTimer 	= this.m_CurrentTime;
			playerEnergy.saveTimer 		= saveTimer;
			playerEnergy.StartCounting ();
			playerEnergy.CalculateEnergy ();
		}

		#endregion

	}
}
