using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CLoadingResourceTask : CSimpleTask {

		private CDownloadResourceManager m_ResourceManager;
		private CUILoading m_UILoading;

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
			this.m_ResourceManager = new CDownloadResourceManager (1, "https://www.dropbox.com/s/nwn0zqcwqvc4360/all_resources.v1?dl=1", true);
#else
			this.m_ResourceManager = new CDownloadResourceManager (1, "https://www.dropbox.com/s/9owavz01m6zeyw5/all_resources.v1?dl=1", true);
#endif
		}

		public override void OnSceneLoaded ()
		{
			base.OnSceneLoaded ();
			this.m_ResourceManager.LoadResource (() => {
				CLog.LogDebug ("Download complete !!");
				this.OnTaskCompleted();
			}, (error) => {
				CLog.LogError (error);
				this.m_IsCompleteTask = false;
			}, (processing) => {
				this.m_UILoading.Processing (processing);
			});
		}
	}
}
