using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pul;

namespace SimpleGameMusic {
	public class CLoadingResourceTask : CSimpleTask {

		private CDownloadResourceManager m_ResourceManager;
		private CUILoading m_UILoading;
		private int m_Version = 1;

		public CLoadingResourceTask () : base ()
		{
			this.taskName = "LoadingResource";
			this.nextTask = "SelectGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UILoading = CUILoading.GetInstance ();
#if UNITY_EDITOR
			this.m_ResourceManager = new CDownloadResourceManager (m_Version, 
				"https://www.dropbox.com/s/xb48sa50bsutvjn/all_resources.v1?dl=1");
#else
			this.m_ResourceManager = new CDownloadResourceManager (m_Version, 
				"https://www.dropbox.com/s/p0pj8zrgp8fd2pf/all_resources.v1?dl=1");
#endif
		}

		public override void OnSceneLoaded ()
		{
			base.OnSceneLoaded ();
			this.DownloadResource();
		}

		private void DownloadResource() {
			this.m_ResourceManager.LoadResource (() => {
				CLog.LogDebug ("Download complete !!");
				this.LoadLanguageCode();
				this.OnTaskCompleted();
			}, (error) => {
				CLog.LogError (error);
				this.m_IsCompleteTask = false;
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
	}
}
