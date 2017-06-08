using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CUISelectGame : CMonoSingleton<CUISelectGame> {

		private CRootTask m_RootTask;

		protected virtual void Start() {
			this.m_RootTask = CRootTask.GetInstance ();
		}

		public void SelectGame(string name) {
			CTask.taskReferences ["SELECTED_GAME"] = name;
			this.m_RootTask.GetCurrentTask ().OnTaskCompleted ();
		}
		
	}
}
