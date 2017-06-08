﻿using System.Collections;
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
			this.m_ResourceManager = new CDownloadResourceManager (0, "http://www.dropbox.com/s/ivrd48us6z2hyts/cactus_leafy_go.go?dl=1", true);
			this.m_ResourceManager.LoadResource (() => {
				Debug.Log ("Download complete !!");
				this.OnTaskCompleted();
			}, (error) => {
				Debug.Log ("Error: " + error);
				this.m_IsCompleteTask = false;
			}, (processing) => {
				this.m_UILoading.Processing (processing);
			});
		}

		public override void UpdateTask (float dt)
		{
			base.UpdateTask (dt);
		}
	}
}
